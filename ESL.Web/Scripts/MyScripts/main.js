var scrollTop = $("#scrollTop");
if ($("main .overlay").length == 0) {
    $(".index-header .menu").css({
        "background": "linear-gradient(95deg, #5533ff 40%, #25ddf5 100%)",
        "position": "fixed",
        "top": "0",
        "padding-bottom": "1rem",
        "box-shadow": "0 2px 4px"
    })
}
$(window).scroll(function () {
    if ($("main .overlay").length == 1) {
        if ($(window).scrollTop() > 10) {
            $(".index-header .menu").css({
                "box-shadow": "0 2px 4px",
                "padding": "1rem 0",
                "background": "linear-gradient(95deg, #5533ff 40%, #25ddf5 100%)"
            })
        } else {
            $(".index-header .menu").css({
                "box-shadow": "unset",
                "padding": "unset",
                "background": "transparent"
            })
        }
    }
    if ($(window).scrollTop() > 250) {
        scrollTop.fadeIn("ease")
    } else {
        scrollTop.fadeOut("ease")
    }
    var clickedMenuItem=$(".navbar .navbar-nav").find("a[href*='#']");
    // if()
});
$(document).ready(function () {
    var clickedMenuItem=$(".navbar .navbar-nav").find("a[href*='#']");
    clickedMenuItem.on("click", function (e) {
        e.preventDefault();
        $("html,body").animate({
            scrollTop: $($(this).attr('href')).offset().top - 150
        }, 400);
    });
    // $(".index-header .menu-item[href='#news-archive']").on("click", function (e) {
    //     e.preventDefault();
    //     $("html,body").animate({
    //         scrollTop: $(this).attr("href").offsetTop
    //     }, 400);
    //     $(this).addClass("active");
    //     $(".index-header .menu-item").not(this).removeClass("active");
    // });

});
scrollTop.on("click", (e) => {
    e.preventDefault();
    $("html,body").animate({scrollTop: 0},
        400);
});


$('.news').owlCarousel({
    loop: true,
    stagePadding: 10,
    nav: true,
    autoplay: true,
    autoplayTimeout: 3000,
    autoplaySpeed: 1000,
    autoplayHoverPause: true,
    navText: ["<span class='far fa-chevron-left'></span>", "<span class='far fa-chevron-right'></span>"],
    responsiveClass: true,
    responsive: {
        0: {
            items: 1,
            margin: 15
        },
        768: {
            items: 2,
            margin: 30
        },
        992: {
            margin: 15,
            items: 3,
            center: true,
        },
        1200: {
            margin: 30
        }
    }
});

$(".list-carousel").owlCarousel({
    loop: true,
    autoplay: true,
    autoplayTimeout: 3000,
    autoplaySpeed: 1000,
    autoplayHoverPause: true,
    center: true,
    responsive: {
        0: {
            items: 5,
            stagePadding: 30,
            margin: 30
        },
        576: {
            items: 7,
            stagePadding: 50,
            margin: 50
        },
        768: {
            items: 7,
            stagePadding: 60,
            margin: 60,
        },
        992: {
            items: 9,
            stagePadding: 60,
            margin: 60
        },
        1200: {
            items: 9,
            stagePadding: 90,
            margin: 90
        }
    }
});

var sync1 = $(".gallery .gallery-slider");
var sync2 = $(".gallery .gallery-thumbs");

var thumbnailItemClass = '.owl-item';
sync1.owlCarousel({
    center: true,
    loop: true,
    autoplay: true,
    autoHeight: false,
    autoplaySpeed: 700,
    autoplayTimeout: 6000,
    navText: ["<span class='far fa-chevron-left'></span>", "<span class='far fa-chevron-right'></span>"],
    nav: true,
    responsive: {
        0: {
            items: 1,
        },
        768: {
            items: 3,
            margin: 0,
        }
    }

}).on('changed.owl.carousel', syncPosition);

function syncPosition(el) {
    $owl_slider = $(this).data('owl.carousel');
    var loop = $owl_slider.options.loop;

    if (loop) {
        var count = el.item.count - 1;
        var current = Math.round(el.item.index - (el.item.count / 2) - .5);
        if (current < 0) {
            current = count;
        }
        if (current > count) {
            current = 0;
        }
    } else {
        var current = el.item.index;
    }

    var owl_thumbnail = sync2.data('owl.carousel');
    var itemClass = "." + owl_thumbnail.options.itemClass;


    var thumbnailCurrentItem = sync2
        .find(itemClass)
        .removeClass("synced")
        .eq(current);

    thumbnailCurrentItem.addClass('synced');

    if (!thumbnailCurrentItem.hasClass('active')) {
        var duration = 300;
        sync2.trigger('to.owl.carousel', [current, duration, true]);
    }
}

var thumbs = sync2.owlCarousel({
    items: 5,
    loop: false,
    margin: 20,
    autoplay: true,
    autoplayTimeout: 6000,
    autoplaySpeed: 700,
    nav: false,
    dots: true,
    onInitialized: function (e) {
        var thumbnailCurrentItem = $(e.target).find(thumbnailItemClass).eq(this._current);
        thumbnailCurrentItem.addClass('synced');
    },
    responsive: {
        0: {
            items: 2
        },
        576: {
            items: 3,
        }
    }
})
    .on('click', thumbnailItemClass, function (e) {
        e.preventDefault();
        var duration = 300;
        var itemIndex = $(e.target).parents(thumbnailItemClass).index();
        sync1.trigger('to.owl.carousel', [itemIndex, duration, true]);
    }).on("changed.owl.carousel", function (el) {
        var number = el.item.index;
        $owl_slider = sync1.data('owl.carousel');
        $owl_slider.to(number, 100, true);
    });
