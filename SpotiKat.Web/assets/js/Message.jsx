var Message = React.createClass({
	setMessage: function(state) {
		this.setState(state);
	},
	getInitialState: function() {
		return {isError: false, isNoAlbums: false};
	},
	render: function() {
		if(this.state.isError) {
			return (
				<div className="message message-failed-load">
					<div>failed to load albums...</div>
					<a onClick={(event) => App.albumsComponent.retry()}>...please try again</a>
				</div>
			);
		} else if(this.state.isNoAlbums) {
			return (
				<div className="message message-no-albums">
					<div>no albums found...</div>
				</div>
			);
		}

		return(
			<div></div>
		);
	}
});
App.messageComponent = React.render(
	<Message />,
	$('#message')[0]
);