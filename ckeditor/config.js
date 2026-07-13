CKEDITOR.editorConfig = function (config) {

	// ==========================
	// CKFinder Configuration
	// ==========================
	config.filebrowserBrowseUrl = '/ckfinder/ckfinder.html';
	config.filebrowserImageBrowseUrl = '/ckfinder/ckfinder.html?type=Images';
	config.filebrowserFlashBrowseUrl = '/ckfinder/ckfinder.html?type=Flash';

	config.filebrowserUploadUrl =
		'/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';

	config.filebrowserImageUploadUrl =
		'/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';

	config.filebrowserFlashUploadUrl =
		'/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';

	// ==========================
	// Toolbar
	// ==========================
	config.toolbar = [
		{ name: 'document', items: ['Source', '-', 'Preview', 'Print'] },
		{ name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
		{ name: 'editing', items: ['Find', 'Replace', '-', 'SelectAll'] },
		'/',
		{ name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', '-', 'RemoveFormat'] },
		{ name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote'] },
		{ name: 'links', items: ['Link', 'Unlink'] },
		{ name: 'insert', items: ['Image', 'Table', 'HorizontalRule', 'SpecialChar'] },
		'/',
		{ name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
		{ name: 'colors', items: ['TextColor', 'BGColor'] },
		{ name: 'tools', items: ['Maximize'] }
	];

	config.height = 350;

	config.removeButtons = '';

	config.extraAllowedContent = 'span(*)[*]{*};img[*]{*}(*)';

	config.removeDialogTabs = 'image:advanced;link:advanced';
};
