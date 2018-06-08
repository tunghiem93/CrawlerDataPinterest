jQuery(document).ready(function($) {
	if (jQuery(".shop_product_slider").length  ) {
		var per_images = 2;


		if(jQuery(window).width() <= 768){
			per_images = 1;
		}


		jQuery(".shop_product_slider").slick({
			autoplay: true,
  			autoplaySpeed: 4000,
			infinite: true,
  			slidesToShow: per_images,
  			slidesToScroll: per_images,
  			dots: true,

		});
	}

	if(jQuery(".layered_dropdown").length){
		jQuery(".layered_dropdown").dropdown();
	}

	jQuery("body").on('click', '.close-dropdownlist', function(event) {
		event.preventDefault();
		/* Act on the event */
		jQuery(this).closest(".layered_dropdown").trigger('click');
	});
});

