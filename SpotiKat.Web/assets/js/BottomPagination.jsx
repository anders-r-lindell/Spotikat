var BottomPagination = React.createClass({
	setPages: function(pages){
		if(pages) {
			this.setState({pages: pages});
		}
    },
	getInitialState: function() {
		return {pages: []};
	},
	render: function() {
		var pages = this.state.pages.map(function (page) {
			if(!page.isDisabled) {
				var className = (page.isCurrent) ? "current" : "";
				return (
					<span>
						<a className={className} onClick={(event) => App.albumsComponent.setPage(page.value)}>{page.text}</a>
					</span>
				);
			} else {
				return (
					<span className="inactive">
						{page.text}
					</span>
				);
			}
		});
		if(pages && pages.length > 0) {
			return (
				<div className="pagination">{pages}</div>
			);
		} else {
			return (
				<div></div>
			);
		}
	}
});
App.bottomPaginationComponent = React.render(
	<BottomPagination />,
	$('#bottomPagination')[0]
);