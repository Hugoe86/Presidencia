$(function(){
	
	$('.boxes').slideToggle("fast");
	$(".box_head").each(function (i) {
        idelem = $(this).attr('id');
		idelemdiv = idelem.replace('boxhead_', 'box_');
		change_boximage(idelem);
    });


	$('.box_head').click(function(){
		idelem = $(this).attr('id');
		idelemdiv = idelem.replace('boxhead_', 'box_');
		$('#'+idelemdiv).slideToggle("normal");
		change_boximage(idelem);
	});

	$('.box_head').hover(
		function() { $(this).css('cursor', 'pointer'); },
		function() { $(this).css('cursor', 'default'); }
	);
});

function change_boximage(elem){
	src = $('#'+elem).attr('src');
	if (src.indexOf("_open") >= 0){
		src = src.replace('_open', '_close');
	} else {
		src = src.replace('_close', '_open');
	}
	src = $('#'+elem).attr('src', src);
}