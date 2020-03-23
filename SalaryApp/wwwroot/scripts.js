jQuery(() => {
    var grids = jQuery('.grid');
    var commonGridOptions = {
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
        textSearch: 'contains',
        // Using a strict parser.
        parser: function (responseText) { return JSON.parse(responseText); },
        //dataType: 'HTMLJSON'
    };

    var tokenSelect = jQuery("input[name = '__RequestVerificationToken'");
    if (tokenSelect.length) commonGridOptions.antiForgeryToken = { __RequestVerificationToken: tokenSelect.val() };

    for (var i = 0; i < grids.length; i++) {
        var gridElement = grids[i];
        var optionsAttribute = gridElement.getAttribute('data-options');
        var options = jQuery.extend({}, commonGridOptions);
        options.name = gridElement.id;
        if (optionsAttribute) jQuery.extend(true, options, JSON.parse(optionsAttribute));
        var grid = jQuery(gridElement).w2grid(options);
        grid.on('add', onGridAddOrEdit);
        grid.on('edit', onGridAddOrEdit);
    }

    function onGridAddOrEdit(event) {
        var grid = this; // w2grid
        var detailUrl = grid.box.getAttribute('data-detail-url');
        if (!detailUrl) {
            event.isCancelled = true;
            return;
        }
        if (event.recid) detailUrl = detailUrl + '?id=' + event.recid;
        var overlay = jQuery('<div class="overlay"><iframe class="window hidden" src="' + detailUrl + '"></iframe></div>');
        overlay.appendTo('body');
        var frame = overlay.find('iframe');
        var frameElement = frame[0];
        var closeOverlay = function () {
            overlay.remove();
            grid.reload();
        };
        overlay.on('click', closeOverlay);
        frame.on('load ', () => {
            var frameDocument = frameElement.contentDocument || frameElement.contentWindow.document;
            // Click on background inside the iframe.
            jQuery(frameDocument).on('click', e => e.target.tagName == 'HTML' ? closeOverlay() : null);
            jQuery(frameElement.contentWindow).on('close', () => jQuery(frameElement).hide());

            var btnCancel = jQuery(frameDocument).find('button.cancel');
            btnCancel.on('click', closeOverlay);
            btnCancel.show();
            jQuery(frameElement).show();
        });
    }
});