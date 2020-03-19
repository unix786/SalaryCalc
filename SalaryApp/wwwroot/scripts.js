jQuery(() => {
    var grids = jQuery('.grid');
    for (var i = 0; i < grids.length; i++) {
        var gridElement = grids[i];
        var options = {
            name: gridElement.id,
            // http://w2ui.com/web/docs/1.5/w2grid.show
            show: {
                toolbar: true,
                toolbarReload: false,
                toolbarAdd: true,
                toolbarDelete: true,
                toolbarSave: true,
                toolbarEdit: true,
                footer: true,
            },
            reorderColumns: true,
            textSearch: "contains",
            // Using a strict parser.
            parser: function (responseText) { return JSON.parse(responseText); },
            postData: {
                //name: gridElement.id
            },
        };
        var optionsAttribute = gridElement.getAttribute("data-options");
        if (optionsAttribute) jQuery.extend(options, JSON.parse(optionsAttribute));
        jQuery(gridElement).w2grid(options);
    }
});