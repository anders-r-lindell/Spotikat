var App = {};

(function ($, window, document, app) {
    'use strict';

    var albumsGridContainer = {
        gridContainer: $('#albums #grid-container'),
        init: function () {
            this.gridContainer.cubeportfolio({
                defaultFilter: '*',
                animationType: 'slideLeft',
                gapHorizontal: 20,
                gapVertical: 20,
                gridAdjustment: 'responsive',
                mediaQueries: [{
                    width: 2000,
                    cols: 7
                },
                {
                    width: 1600,
                    cols: 6
                },{
                    width: 1200,
                    cols: 5
                }, {
                    width: 800,
                    cols: 4
                }, {
                    width: 500,
                    cols: 2
                }, {
                    width: 320,
                    cols: 1
                }],
                caption: 'zoom',
                displayType: 'lazyLoading',
                displayTypeSpeed: 100
            });
        },
        destroy: function () {
            this.gridContainer.cubeportfolio('destroy');
        }
    };

    app.albumsGridContainer = albumsGridContainer;

    $(document).ready(function () {
        app.albumsGridContainer.init();
    });

    App.albumsComponent = {};
    App.messageComponent = {};
    App.genresComponent = {};
    App.topPaginationComponent = {};
    App.bottomPaginationComponent = {};

    var locationHashResolver = {
        resolve: function () {
            var hash = window.location.hash;
            if (hash !== "") {
                var hashSegments = hash.split("/");
                if (hashSegments.length === 5) {
                    var genreSegment = hashSegments[2];
                    var genre = {};
                    for (var i = 0; i < App.genresComponent.state.data.genres.length; i++) {
                        if (App.genresComponent.state.data.genres[i].value === genreSegment) {
                            genre = App.genresComponent.state.data.genres[i];
                            break;
                        }
                    }

                    return { genre: genre, page: hashSegments[3] };
                }
            } 
            
            return { genre: { text: '> CHOOSE YOUR GENRE HERE <', isLastAlbumRoute: true, source: 0 }, page: 1 };
        }
    };

    App.locationHashResolver = locationHashResolver;

    $(window).on("popstate", function () {
        var genreAndPage = App.locationHashResolver.resolve();
        App.genresComponent.setState({ selectedGenre: genreAndPage.genre });
        App.albumsComponent.setGenreAndPage(genreAndPage.genre, genreAndPage.page);
    });

})(jQuery, window, document, App);
