$('a.scroll').click(function(e){
	e.preventDefault();
	$('html, body').stop().animate({scrollTop: $($(this).attr('href')).offset().top}, 1000);
});


$(window).scroll(function () {
    if ($(this).scrollTop() > 100) {
        $('.scrollup').fadeIn();
    } else {
        $('.scrollup').fadeOut();
    }
});
$('.scrollup').click(function () {
    $("html, body").animate({ scrollTop: 0 }, 1000);
    return false;
});
