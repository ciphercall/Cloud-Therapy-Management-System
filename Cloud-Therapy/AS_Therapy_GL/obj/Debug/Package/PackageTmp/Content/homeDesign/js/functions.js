$(document).ready(function() {
    var width=$(window).width();
    /*Focusing section from front page*/
    function gotoPositon() {
        $('html,body').animate({
            scrollTop: '-=200px'
        })
    }
    var currentSection = $.cookie("section");
    switch (currentSection) {
        case 'benifits':
            $('a[href=#benefits]')[0].click();
            gotoPositon();
            break;
        case 'features':
            $('a[href=#features]')[0].click();
            gotoPositon();
            break;
        case 'chooseUs':
            $('a[href=#chooseUs]')[0].click();
            gotoPositon();
            break;
        case 'service':
            $('a[href=#service]')[0].click();
            gotoPositon();
            break;
    }

    $('a').click(function(event) {
        var section = $(this).attr('id');
        $.cookie("section", section);
    });

    /*End focusing section from front page*/

    $("#owl-rs").owlCarousel({
        navigation: true, // Show next and prev buttons
        slideSpeed: 300,
        paginationSpeed: 400,
        singleItem: true,
        navigation: false,
        pagination: false,
        //Autoplay
        autoPlay: true,
        stopOnHover: true,
        //Auto height
        autoHeight: false,

        // "singleItem:true" is a shortcut for:
        // items : 1,
        // itemsDesktop : false,
        // itemsDesktopSmall : false,
        itemsTablet: false,
        itemsMobile: false
    });

    // $("#owl-clients").owlCarousel({
    //     navigation: true, // Show next and prev buttons
    //     itemsMobile: [479, 1],
    //     slideSpeed: 200,
    //     rewindSpeed: 1000,
    //     paginationSpeed: 400,
    //     singleItem: false,
    //     navigation: false,
    //     pagination: false,
    //     //Autoplay
    //     autoPlay: true,
    //     stopOnHover: true,
    //     //Auto height
    //     autoHeight: false,

    //     // "singleItem:true" is a shortcut for:
    //     items: 12,
    //     // itemsDesktop : false,
    //     // itemsDesktopSmall : false,
    //     itemsTablet: false,
    //     itemsMobile: false
    // });

    //start remote cliets data
    $("#owl-clients").owlCarousel({
        jsonPath: 'http://192.168.0.78:141/alchmey/client-api',
        jsonSuccess: customDataSuccess,
        navigation: true, // Show next and prev buttons
        itemsMobile: [479, 1],
        slideSpeed: 200,
        rewindSpeed: 1000,
        paginationSpeed: 400,
        singleItem: false,
        navigation: false,
        pagination: false,
        //Autoplay
        autoPlay: true,
        stopOnHover: true,
        //Auto height
        autoHeight: false,
        // "singleItem:true" is a shortcut for:
        items: 12,
        // itemsDesktop : false,
        // itemsDesktopSmall : false,
        itemsTablet: false,
        itemsMobile: false
    });

    function customDataSuccess(data) {
            var content = "";
            for (var i in data) {
                var img = data[i].logo;
                var clientName = data[i].clientName;
                content += "<img src=\"" + img + "\" alt=\"" + clientName + "\" title=" + clientName + " data-toggle=\"tooltip\" data-placement=\"top\">"
            }
            $("#owl-clients").html(content);
        }
    //end remote cliets data

    //sticky about us nav
    var num = 50; //number of pixels before modifying styles
    $(window).bind('scroll', function() {
        if ($(window).scrollTop() > num && width>1000) {
            $('#secondaryHeader').removeClass('animated fadeInUp');
            $('#secondaryHeader').addClass('animated fadeInDown');

            $('#secondaryHeader').css({
                display: 'block',
                position: 'fixed',
                top: '0px',
                left: '0px',
                right: '0px',
                zIndex: '500'
            });

        } else {

            $('#secondaryHeader').removeClass('animated fadeInDown');
            $('#secondaryHeader').addClass('animated fadeInUp');

            $('#secondaryHeader').hide();

        }
    });

    //end sticky about us nav

    //smooth scroll
    $(function() {
        $('#secondaryHeader a[href*=#]:not([href=#])').click(function() {
            if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
                var target = $(this.hash);
                target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
                if (target.length) {
                    $('html,body').animate({
                        scrollTop: target.offset().top - 200
                    }, 1000);
                    return false;
                }
            }
        });
    });

    //click to top
    jQuery(window).scroll(function() {
        if (jQuery(this).scrollTop() > 100) {
            jQuery('#backToTop').fadeIn('slow');
        } else {
            jQuery('#backToTop').fadeOut('slow');
        }
    });
    jQuery('#backToTop').click(function() {
        jQuery("html, body").animate({
            scrollTop: 0
        }, 500);
        return false;
    });

    //end click to top

    //highlight current menu
    var url = window.location.href;
    $('#leftMenu ul li a').filter(function () {
        return this.href == url;
    }).addClass('active')

     $("#secondaryHeader ul li a").click(function(event) {
         $('#secondaryHeader ul li a').removeClass('active');
         $(this).addClass('active');
     });

     //end highlight menu

     $(function(){
        jQuery('[data-toggle="tooltip"]').tooltip();
     })

});
