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
    public class AlbumsApiController : ApiController {
        private readonly IAlbumService _albumService;
        private readonly INewRelicTransactionManager _newRelicTransactionManager;
        private readonly ILogFactory _logFactory;

        public AlbumsApiController(IAlbumService albumService, INewRelicTransactionManager newRelicTransactionManager, 
            ILogFactory logFactory) {
            _albumService = albumService;
            _newRelicTransactionManager = newRelicTransactionManager;
            _logFactory = logFactory;
        }

        [HttpGet]
        [Route("api/albums/{source}/{genre}/{page}/")]
        public async Task<AlbumsResponse> Get([FromUri] AlbumsRequest request) {
            AddNewRelicCustomParameters(request);

            var albumsResponse = new AlbumsResponse {
                Albums = new List<Album>(),
                Pages = new List<Page>(),
                Info = new AlbumsResponseInfo {Genre = request.Genre, Page = request.Page, Source = request.Source}
            };
            
            try {
                ValidateRequest(request);
                var albums = await _albumService.GetAlbumsByGenreAsync(request.Source, request.Genre, request.Page);

                albumsResponse.Albums = albums.Items;
                albumsResponse.Pages = albums.Pages;
                albumsResponse.Info.Count = albums.Items.Count;
                albumsResponse.ResponseStatusCode = HttpStatusCode.OK;

                return albumsResponse;
            }
            catch (ServiceApiException saex) {
                LogError(saex);
                albumsResponse.ResponseStatusCode = saex.StatusCode;
                return albumsResponse;
            }
            catch (Exception ex) {
                LogError(ex);
                albumsResponse.ResponseStatusCode = HttpStatusCode.InternalServerError;
                return albumsResponse;
            }
        }

        private void LogError(Exception ex)
        {
            _logFactory.GetLogger(typeof(GenresApiController)).ErrorFormat("Request failed for '{0}': {1}", Request.RequestUri.PathAndQuery, ex.Message, ex);
            _newRelicTransactionManager.NoticeError(ex);
        }

        private void AddNewRelicCustomParameters(AlbumsRequest request) {
            _newRelicTransactionManager.AddCustomParameter("request.Source", request.Source);
            _newRelicTransactionManager.AddCustomParameter("request.Genre", request.Genre);
            _newRelicTransactionManager.AddCustomParameter("request.Page", request.Page);
        }

        private void ValidateRequest(AlbumsRequest request) {
            if (string.IsNullOrWhiteSpace(request.Genre)) {
                throw new ServiceApiException(HttpStatusCode.BadRequest,
                    string.Format(ServiceApiException.InvalidValueErrorMessageFormat, "Genre", request.Genre));
            }
            if (request.Page <= 0) {
                throw new ServiceApiException(HttpStatusCode.BadRequest,
                    string.Format(ServiceApiException.InvalidValueErrorMessageFormat, "Page", request.Page));
            }
        }
    }
}