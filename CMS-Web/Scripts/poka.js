jQuery(document).ready(function(){

// ==================== slider home --=========================

if(jQuery('.slider-top-home').length){
  jQuery('.slider-top-home').slick({
    dots: true,
    infinite: true,
    speed: 300,
    slidesToShow: 1,
    adaptiveHeight: true,
    autoplay: true,
    autoplaySpeed: 2000,
  });
}
// ================ snd slider =============================== 

  // =================== check out ===========
  if(jQuery(".woocommerce-checkout").length){
   jQuery("body").on("click","#place_order",function(){
     jQuery(".custom_error").css("display","block");
   } );

 }
  // =========== product =========
 

    jQuery(".hidden-text .readmore").click(function(){
        jQuery(".content-hidden-product").css("max-height","100%");
        jQuery(".hidden-text .readmore").css("display","none");
    });
  

  // ===============================
  if(jQuery('.woocommerce-cart').length){
    jQuery("body").on("change", "input.input-text.qty.text", function(){
      jQuery('.actions .button[name="update_cart"]').trigger('click');
    });
  }

  if(jQuery('.bagi_price_.bagi_price_out_stock').length){
   jQuery('button.single_add_to_cart_button').click(function(){
    return false;
  });
   jQuery('button.buynow_product').click(function(){
    return false;
  });
   jQuery('button.buynow_product').addClass('flase');
   jQuery('button.single_add_to_cart_button').addClass('flase');
 }
 if (jQuery('.single.single-product .rating-s').length) {
  jQuery('.no-rating-sku').css('display', 'none');;
}
// ====================
if(jQuery('.toggler-dropdow').length){
  jQuery('.toggler-dropdow').click(function(){
    jQuery('.toggler-block').show();
    jQuery('.toggler-dropdow').addClass('toggler-hidden');
  });
  jQuery('.toggler-hidden').click(function(){

    jQuery('.toggler-block').hide();
    jQuery('.toggler-dropdow').removeClass('toggler-hidden');
  });

}
var is_open = 0;
jQuery("body").on('click', '.menu-toggle', function(event) {
  if( !is_open ){

    jQuery("#menu-menu-mobile").animate({"left": 0}, "slow");
    is_open = 1;
  }
  else{
   jQuery("#menu-menu-mobile").animate({"left": -700}, "slow");
   is_open = 0;
 }
});
// ==========================
var price_1 = jQuery('.price').html();
 jQuery(".price_custom .bagi_price").html('<b>Giá cả:</b> '+ price_1);
jQuery("body").on('change', '.variations_custom select', function(event) {
  event.preventDefault();
  /* Act on the event */
  var price = jQuery("ins .woocommerce-Price-amount.amount").html();

  jQuery(".price_custom .bagi_price").html('<b>Giá cả:</b> '+ price);
});
// ==================================
// if (jQuery('.comment-form-comment').length) {
//   var form_comment = jQuery(".comment-form-comment textarea").html();
//   jQuery(".comment-form-comment textarea").html('.');
// }
jQuery('.stars').click(function(){
  jQuery( ".comment-form .form-submit #submit" ).trigger( "click" );
})

// ===========custom acount=============

// ====================== menu dropdow =======
var b_screen = jQuery(window).width();
if (b_screen <= 480) {
  jQuery("#menu-menu-mobile li").hover(
    function(){
      jQuery(this).children('ul').hide();
      jQuery(this).children('ul').slideDown('fast');
    },
    function () {
      jQuery('ul', this).slideUp('fast');            
    });
  jQuery('#menu-menu-mobile li').hover(
    function(){
      jQuery(this).addClass('focus');
    },function(){
      jQuery(this).removeClass('focus');
    }
    )
}

// ==================== menu-toggle ===========
if (b_screen <= 480) {
  jQuery("body").on('click', 'button.menu-toggle', function(event) {
    jQuery('.sticky-wrapper .site-header').toggleClass('overlay');
  });

}
// =========================================

// ===================
if (jQuery('.menu-category-top').length){
  jQuery('.menu-category-top').slick({
    speed: 300,
    slidesToShow: 4,
    slidesToScroll: 4,
    autoplay: true,
    autoplaySpeed: 3000,
  });
}
// ================== show-hidden-button ===============
if (jQuery('.show-hidden-button').length) {
 jQuery('body').on("click", ".show-hidden-button", function(){
  jQuery('.product_search form button.search_submit').show();
  jQuery('.product_search form input.product-search-field').show();
  jQuery('.show-hidden-button').addClass('hidden-button');
});
 jQuery('body').on("click", ".show-hidden-button.hidden-button", function(){
  jQuery('.product_search form button.search_submit').hide();
  jQuery('.product_search form input.product-search-field').hide();
  jQuery('.show-hidden-button').removeClass('hidden-button');
});

}

// =======================================================
var target;
jQuery(".dropdow-sub-menu-left-child").hover(function(){
  target = jQuery(this).data("target");
  jQuery(".sub-menu-child").parent().hide();

  jQuery(".sub-menu-"+target).parent().show(); 
});
// ====================================================================
jQuery(window).scroll(function(){
  if ( jQuery(this).scrollTop() > 250 && !jQuery('#myBtn').hasClass('btn_show') ) {

    jQuery('#myBtn').addClass('btn_show');
  } 

  if( jQuery(this).scrollTop() < 250 && jQuery('#myBtn').hasClass('btn_show') ) {
    jQuery('#myBtn').removeClass('btn_show');
  }
});

  //Click event to scroll to top
  jQuery('body').on("click", "#myBtn", function(){
    jQuery('html, body').animate({scrollTop : 0},600);
    return false;
  });
  // =====================================
  if (b_screen >= 680){
     jQuery(".tabs.wc-tabs").sticky({

    topSpacing:80,
    'stopper' : '.related.products',

  });

  jQuery(".site-header").sticky({

    topSpacing:0,
    'stopper' : '.related.products',

  });

  }
 

});
