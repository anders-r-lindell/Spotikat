using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using SpotiKat.Api.ServiceInterface.Exceptions;
using SpotiKat.Api.ServiceModel.Request;
using SpotiKat.Api.ServiceModel.Response;
using SpotiKat.Entities;
using SpotiKat.Interfaces.Logging;
using SpotiKat.NewRelic.Interfaces;
using SpotiKat.Services.Interfaces;

namespace SpotiKat.Api.ServiceInterface {
    public class LastAlbumsApiController : ApiController {
        private readonly ILastAlbumService _lastAlbumService;
        private readonly INewRelicTransactionManager _newRelicTransactionManager;
        private readonly ILogFactory _logFactory;

        public LastAlbumsApiController(ILastAlbumService lastAlbumService,
            INewRelicTransactionManager newRelicTransactionManager, ILogFactory logFactory) {
            _lastAlbumService = lastAlbumService;
            _newRelicTransactionManager = newRelicTransactionManager;
            _logFactory = logFactory;
        }

        [HttpGet]
        [Route("api/lastalbums/{source}/{page}/")]
        public async Task<LastAlbumsResponse> Get([FromUri] LastAlbumsRequest request) {
            AddNewRelicCustomParameters(request);

            var lastAlbumsResponse = new LastAlbumsResponse {
                Albums = new List<Album>(),
                Info = new LastAlbumsResponseInfo {Page = request.Page, Source = request.Source}
            };

            try {
                ValidateRequest(request);
                var albums = await _lastAlbumService.GetFeedItemsAlbumsAsync(request.Source, request.Page);

                lastAlbumsResponse.Albums = albums;
                lastAlbumsResponse.Info.Count = albums.Count;
                lastAlbumsResponse.ResponseStatusCode = HttpStatusCode.OK;
                
                return lastAlbumsResponse;
            }
            catch (ServiceApiException saex) {
                LogError(saex);
                lastAlbumsResponse.ResponseStatusCode = saex.StatusCode;
                return lastAlbumsResponse;
            }
            catch (Exception ex) {
                LogError(ex);
                lastAlbumsResponse.ResponseStatusCode = HttpStatusCode.InternalServerError;
                return lastAlbumsResponse;
            }
        }

        private void LogError(Exception ex) {
            _logFactory.GetLogger(typeof(LastAlbumsApiController)).ErrorFormat("Request failed for '{0}': {1}", Request.RequestUri.PathAndQuery, ex.Message, ex);
            _newRelicTransactionManager.NoticeError(ex);
        }

        private void ValidateRequest(LastAlbumsRequest request) {
            if (request.Page < 1) {
                throw new ServiceApiException(HttpStatusCode.BadGateway,
                    string.Format(ServiceApiException.InvalidValueErrorMessageFormat, "Page", request.Page));
            }
        }

        private void AddNewRelicCustomParameters(LastAlbumsRequest request) {
            _newRelicTransactionManager.AddCustomParameter("request.Source", request.Source);
            _newRelicTransactionManager.AddCustomParameter("request.Page", request.Page);
        }
    }
}