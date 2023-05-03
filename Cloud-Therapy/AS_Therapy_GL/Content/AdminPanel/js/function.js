 //Theme selection
 //var themeName = "Default";
 //if ($.cookie('activeTheme') == null) {
 //    $('#themeStyle').attr('href', '/Content/AdminPanel/css/Theme/Default.css');
 //} else {
 //    themeName = $.cookie("activeTheme");
 //    $('#themeStyle').attr('href', '/Content/AdminPanel/css/Theme/' + themeName + '.css');
 //}

 //end theme selection

 $(document).ready(function() {
     var newWidth = $(window).width();
     var newHeight = ($(window).height());
     console.log("Screen Height :" + newHeight + "");

     $('#sidebar').css("min-height", "" + newHeight - 70 + "px");
     $('#sidebar').css("padding-bottom", "60px");
   //  $('#contentBox').css("height", "" + newHeight - 82 + "");

     $('#menuExpand').hide();

     $("#responsiveMenu").click(function() {
         $("#sidebar").toggle('slow');
         return false;
     });

     $('#normalView').click(function(event) {
         $('#menuCollapse').toggle();
         $('#sidebar').slideToggle("slow");

         if ($("#mainContentBox").hasClass('col-md-10')) {
             $("#mainContentBox").removeClass('col-md-10');
             $("#mainContentBox").addClass('col-md-12');
             $('#contentBox').css('margin-left', '-15px');
         } else {
             $("#mainContentBox").removeClass('col-md-12');
             $("#mainContentBox").addClass('col-md-10');
             $('#contentBox').css('margin-left', '-3px');
         }


     });

     /*hover accordin*/
     /* jQuery(function($) {
              $('.accordion li').hover(function() {
                  $(this).find('ul').stop(true, true).slideDown('slow')
              }, function() {
                  $(this).find('ul').stop(true, true).slideUp('slow')
              }).find('ul').hide()
          })*/
     /*hover accordin end */


     /*click to expand menu*/
     $('.accordion').children('li').on('click', function() {
         $(this).children('ul').slideToggle('slow');
     });
     /*end menu expand*/

     /*active class add*/
     $("#sidebar ul li").click(function(event) {
         $('#sidebar ul li').removeClass('active');
         $(this).addClass('active');
     });



     $('#menuCollapse').click(function(event) {
         $("#sidebar").css({
             width: '50px',
         });
         $(this).fadeOut();
         $('#menuExpand').fadeIn();

         $("#sidebar ul li").find(".mainMenuText").fadeOut();
         $("#sidebar ul li").find(".notification").fadeOut();
         $('#contentBox').parent().removeClass('col-md-10');
         $('#contentBox').parent().addClass('col-md-11').css("width", "95.667%");

         $('.subMenu li').css({
             width: '180px',
             zIndex: 100,
         });

         $('#developer').hide();

     });

     $('#menuExpand').click(function(event) {
         $(this).fadeOut();
         $('#menuCollapse').fadeIn();

         $("#sidebar").attr("style", "");
         $('#sidebar').css("height", "" + newHeight - 70 + "");
         $("#sidebar ul li").find(".mainMenuText").fadeIn();
         $("#sidebar ul li").find(".notification").fadeIn();

         $('#contentBox').parent().removeClass('col-md-11');
         $('#contentBox').parent().addClass('col-md-10');
         $('#contentBox').parent().attr("style", "");

         $('.subMenu li').css({
             width: '100%',
             zIndex: 100,
         });

         $('#developer').fadeIn();

     });

     $(function() {
         //Better to construct options first and then pass it as a parameter
         var options = {
             animationEnabled: true,
             data: [{
                 type: "spline", //change it to line, area, column, pie, etc
                 dataPoints: [{
                     x: 10,
                     y: 10
                 }, {
                     x: 20,
                     y: 12
                 }, {
                     x: 30,
                     y: 8
                 }, {
                     x: 40,
                     y: 14
                 }, {
                     x: 50,
                     y: 6
                 }, {
                     x: 60,
                     y: 24
                 }, {
                     x: 70,
                     y: -4
                 }, {
                     x: 80,
                     y: 10
                 }]
             }]
         };

         //$("#homeChart").CanvasJSChart(options);

     });





     //popup section
     $('#developerLogo,#developerName').hover(function(e) {
         $('div#pop-up').css({
             display: 'block',
         });
     });

     $('#closePopUp').click(function(event) {
         $('div#pop-up').css({
             display: 'none',
         });
     });
     //end pop up section

     //theme selection
     $('#themesThum td').click(function(event) {
         var name = $(this).find('p').text();
         $.cookie("activeTheme", "" + name + "", {
             expires: 30
         });

        location.reload(); 
     });
     //theme selection end


 }); //document function ready end
