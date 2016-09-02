using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using SpotiKat.Api.ServiceInterface.Exceptions;
using SpotiKat.Api.ServiceModel.Response;
using SpotiKat.Entities;
using SpotiKat.Interfaces.Logging;
using SpotiKat.NewRelic.Interfaces;
using SpotiKat.Services.Interfaces;

namespace SpotiKat.Api.ServiceInterface {
    public class GenresApiController : ApiController {
        private readonly IGenreService _genreService;
        private readonly INewRelicTransactionManager _newRelicTransactionManager;
        private readonly ILogFactory _logFactory;

        public GenresApiController(IGenreService genreService, INewRelicTransactionManager newRelicTransactionManager, 
            ILogFactory logFactory) {
            _genreService = genreService;
            _newRelicTransactionManager = newRelicTransactionManager;
            _logFactory = logFactory;
        }

        [HttpGet]
        [Route("api/genres/")]
        public GenresResponse Get() {
            var genresResponse = new GenresResponse {
                Genres = new List<Genre>()
            };

            try {
                genresResponse.Genres = _genreService.GetGenres();
                genresResponse.ResponseStatusCode = HttpStatusCode.OK;

                return genresResponse;
            }
            catch (ServiceApiException saex) {
                LogError(saex);
                genresResponse.ResponseStatusCode = saex.StatusCode;
                return genresResponse;
            }
            catch (Exception ex) {
                LogError(ex);
                genresResponse.ResponseStatusCode = HttpStatusCode.InternalServerError;
                return genresResponse;
            }
        }

        private void LogError(Exception ex)
        {
            _logFactory.GetLogger(typeof(GenresApiController)).ErrorFormat("Request failed for '{0}': {1}", Request.RequestUri.PathAndQuery, ex.Message, ex);
            _newRelicTransactionManager.NoticeError(ex);
        }
    }
}