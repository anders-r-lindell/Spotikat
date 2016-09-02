var Albums = React.createClass({
	setGenreAndPage: function(genre, page) {
		if(this.state.isLoading) {
			return;
		}
		this.genre = genre;
		this.page = page;
		App.messageComponent.setMessage({isError: false, isNoAlbums: false});
		App.albumsGridContainer.destroy();
		this.setState({data: {albums: [], pages: [], responseStatusCode: null}, isLoading: true});
		this.loadAlbums(false);
	},
	setGenre: function(genre) {
		if(this.state.isLoading) {
			return;
		}
		this.genre = genre;
		this.page = 1;
		App.messageComponent.setMessage({isError: false, isNoAlbums: false});
		App.albumsGridContainer.destroy();
		this.setState({data: {albums: [], pages: [], responseStatusCode: null}, isLoading: true});
		this.loadAlbums(true);
	},
	setPage: function(page) {
		if(this.state.isLoading) {
			return;
		}
		this.page = page;
		App.messageComponent.setMessage({isError: false, isNoAlbums: false});
		App.albumsGridContainer.destroy();
		this.setState({data: {albums: [], pages: [], responseStatusCode: null}, isLoading: true});
		this.loadAlbums(true);
	},
	retry: function() {
		if(this.state.isLoading) {
			return;
		}
		App.messageComponent.setMessage({isError: false, isNoAlbums: false});
		App.albumsGridContainer.destroy();
		this.setState({data: {albums: [], pages: [], responseStatusCode: null}, isLoading: true});
		this.loadAlbums(false);
	},
	loadAlbums: function(pushState) {
		var url = "";
		if(this.genre.isLastAlbumRoute) {
			url = this.props.lastAlbumsUrl + this.genre.source + "/" + this.page + "/";
		} else {
			url = this.props.albumsUrl + this.genre.source + "/" + this.genre.value + "/" + this.page + "/";
		}
		if(pushState && this.genre.value) {
			history.pushState({}, '', "#albums/" + this.genre.source + "/" + this.genre.value + "/" + this.page + "/");
		}
		$.ajax({
			context: this,
			url: url,
			dataType: 'json',
			success: function(data) {
				this.setState({data: data, isLoading: false});
				if(data.albums.length == 0) {
					App.messageComponent.setMessage({isError: false, isNoAlbums: true});
				}
				if(data.responseStatusCode !== 200) {
					App.messageComponent.setMessage({isError: true, isNoAlbums: false});
				}
			}.bind(this),
			error: function() {
				this.setState({data: {albums: [], pages: [], responseStatusCode: null}, isLoading: false});
				App.messageComponent.setMessage({isError: true, isNoAlbums: false});
			}
		});
	},
	getInitialState: function() {
		return {data: {albums: [], pages: []}, isLoading: false};
	},
	componentDidUpdate: function() {
		if(!this.state.isLoading) {
			App.albumsGridContainer.init();
		}
		if(App.topPaginationComponent.setPages) {
			App.topPaginationComponent.setPages(this.state.data.pages);
		}
		if(App.bottomPaginationComponent.setPages) {
			App.bottomPaginationComponent.setPages(this.state.data.pages);
		}
		if(App.messageComponent.setMessage) {
			App.messageComponent.setMessage({isError: false, isNoAlbums: false});
		}
	},
	render: function() {
		var albums = this.state.data.albums.map(function (album) {
			return (
				<div className="cbp-item graphic">
					<a href={album.href} className="cbp-caption">
						<div className="cbp-caption-defaultWrap">
							<img src={album.imageUrl} alt=""/>
						</div>
						<div className="cbp-caption-activeWrap">
							<div className="cbp-l-caption-alignCenter">
								<div className="cbp-l-caption-body">
									<img src="/assets/img/play.png" className="play" />
								</div>
							</div>
						</div>
						<div className="cbp-caption-albumInfoWrap">
							<div className="cbp-l-caption-body">
								<div className="cbp-l-caption-title">{album.artist}</div>
								<div className="cbp-l-caption-desc">{album.name}</div>
							</div>
						</div>
					</a>
				</div>
			);
		});
		if(!this.state.isLoading && this.state.data.responseStatusCode === 200) {
			return (
				<div>
					{albums}
				</div>
			);
		} else {
			return (
				<div>
				</div>
			);
		}
	}
});
App.albumsComponent = React.render(
	<Albums albumsUrl="/api/albums/" lastAlbumsUrl="/api/lastalbums/" />,
	$('#grid-container')[0]
);