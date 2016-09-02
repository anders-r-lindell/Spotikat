var Genres = React.createClass({
	setToggle: function(){
		if(this.state.toggle === "off") {
			this.setState({toggle: "on"});
		} else {
			this.setState({toggle: "off"});
		}
    },
	setGenre: function(genre) {
		if(App.albumsComponent.state.isLoading) {
			this.setState({toggle: "off"});
		} else {
			this.setState({toggle: "off", selectedGenre: genre});
		}
		App.albumsComponent.setGenre(genre);
	},
	getInitialState: function() {
		return {data: {genres: []}, toggle: "off", selectedGenre: { }, isLoading: true};
	},
	componentWillMount: function() {
		$.ajax({
			url: this.props.url,
			dataType: 'json',
			success: function(data) {
				this.setState({data: data, isLoading: false});
				var genreAndPage = App.locationHashResolver.resolve();
				this.setState({selectedGenre: genreAndPage.genre});
				App.albumsComponent.setGenreAndPage(genreAndPage.genre, genreAndPage.page);
			}.bind(this)
		});
	},
	componentDidUpdate: function() {
	},
	render: function() {
		var that = this;
		var genres = this.state.data.genres.map(function (genre) {
			if(genre.text !== ""){
				return (
					<li onClick={(event) => that.setGenre(genre)}>{genre.text}</li>
				);
			} else {
				return (
					<li className="empty">&nbsp;</li>
				);
			}
		});
		if(this.state.isLoading) {
			return (
				<div className="selected-genre">&nbsp;</div>
			);
		} else {
			if(this.state.toggle === "on") {
				return (
					<div>
						<div className="selected-genre" onClick={this.setToggle.bind()}>{this.state.selectedGenre.text}</div>
						<ul>
							{genres}
						</ul>
					</div>
				);
			} else {
				return (
					<div>
						<div className="selected-genre" onClick={this.setToggle.bind()}>{this.state.selectedGenre.text}</div>
					</div>
				);
			}
		}
	}
});
App.genresComponent = React.render(
	<Genres url="/api/genres/" />,
	$('#genres')[0]
);