var App = {};

(function ($, window, document, app) {
    'use strict';

    var albumsGridContainer = {
        gridContainer: $('#grid-container'),
        init: function() {
            this.gridContainer.cubeportfolio({
                defaultFilter: '*',
                animationType: 'slideLeft',
                gapHorizontal: 20,
                gapVertical: 20,
                gridAdjustment: 'responsive',
                mediaQueries: [{
                    width: 1500,
                    cols: 5
                }, {
                    width: 1100,
                    cols: 4
                }, {
                    width: 800,
                    cols: 3
                }, {
                    width: 480,
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
        destroy: function() {
            this.gridContainer.cubeportfolio('destroy');
        }
    };

    app.albumsGridContainer = albumsGridContainer;

    $(document).ready(function () {
        app.albumsGridContainer.init();
    });

    App.albumsComponent = {};
    App.amessageComponent = {};
    App.albumsComponent = {};
    App.genresComponent = {};

    var genreLocationHashResolver = {
        resolve: function() {
            
        }
    };

    App.genreLocationHashResolver = genreHashResolver;

    $(window).on("popstate", function () {
        var hash = window.location.hash;
        if (hash !== "") {
            var hashSegments = hash.split("/");
            if (hashSegments.length === 5) {
                var genreSegment = hashSegments[2];
                var genre = {};
                for (var i = 0; i < genresComponent.state.data.genres.length; i++) {
                    if (genresComponent.state.data.genres[i].value === genreSegment) {
                        genre = genresComponent.state.data.genres[i];
                        break;
                    }
                }
                genresComponent.setState({ selectedGenre: genre });
                var page = hashSegments[3];
                albumsComponent.setGenreAndPage(genre, page);
            }
        } else {
            albumsComponent.setGenre({ isLastAlbumRoute: true, source: 0 });
        }
    });

})(jQuery, window, document, App);
