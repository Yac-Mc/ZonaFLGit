jQuery(function ($) {
    'use strict',

    //#main-slider
        $(function () {
            $('#main-slider.carousel').carousel({
                interval: 8000
            });
        });


    // accordian
    $('.accordion-toggle').on('click', function () {
        $(this).closest('.panel-group').children().each(function () {
            $(this).find('>.panel-heading').removeClass('active');
        });

        $(this).closest('.panel-heading').toggleClass('active');
    });

    //Initiat WOW JS
    new WOW().init();

    // portfolio filter
    $(window).load(function () {
        'use strict';
        var $portfolio_selectors = $('.portfolio-filter >li>a');
        var $portfolio = $('.portfolio-items');
        $portfolio.isotope({
            itemSelector: '.portfolio-item',
            layoutMode: 'fitRows'
        });

        $portfolio_selectors.on('click', function () {
            $portfolio_selectors.removeClass('active');
            $(this).addClass('active');
            var selector = $(this).attr('data-filter');
            $portfolio.isotope({ filter: selector });
            return false;
        });
    });

    // Contact form
    var form = $('#main-contact-form');
    form.submit(function (event) {
        event.preventDefault();
        var form_status = $('<div class="form_status"></div>');
        $.ajax({
            url: $(this).attr('action'),

            beforeSend: function () {
                form.prepend(form_status.html('<p><i class="fa fa-spinner fa-spin"></i> Email is sending...</p>').fadeIn());
            }
        }).done(function (data) {
            form_status.html('<p class="text-success">' + data.message + '</p>').delay(3000).fadeOut();
        });
    });








    //goto top
    $('.gototop').click(function (event) {
        event.preventDefault();
        $('html, body').animate({
            scrollTop: $("body").offset().top
        }, 500);
    });

    //Pretty Photo
    $("a[rel^='prettyPhoto']").prettyPhoto({
        social_tools: false
    });
});




// Función reemplazo de div categorias
var category = null;
$(".change").click(function () {

    $("#" + this.id.split("-")[1]).show("1500");
    $("#8_categorias").hide("slow");
    //category = this.id.split("-")[1]
    category = $("#" + this.id.split("-")[1])[0].innerText;

});

$(".return").click(function () {
    $("#" + this.id.split("-")[1]).hide("slow");
    $("#8_categorias").show("slow");
    category = null;
})

// Funcionalidad conteo de skills en vista de edición





var data = { skills: [], company: [] };
function toggleCheckbox(element) {

    if (element.id.indexOf("-") > 0) {

        if ($(element).is(':checked')) {
            $(element).parents(".padre").addClass("checkchecked");
            $("#" + element.id.split("-")[1]).css("display", "inline-block");
            var id = element.id.split("-")[1];
            var name = $("#" + element.id.split("-")[1]).text();
            var categorySkill = category;
            var idheader = $("#" + id).parent().parent()[0].id;
            var currentCount = parseInt($("#" + idheader + "count").html().replace("(", "").replace(")", ""));
            var currentCountAll = parseInt($("#allcatcount").html().replace("(", "").replace(")", ""));
            currentCount++;
            currentCountAll++;
            $("#" + idheader + "count").html("(" + currentCount + ")");
            $("#allcatcount").html("(" + currentCountAll + ")");

            data.skills.push({ Id: id, Name: name, CategorySkill: categorySkill, IdHtml: id });
        } else {
            $(element).parents(".padre").removeClass("checkchecked");
            $("#" + element.id.split("-")[1]).css("display", "none");
            var id = element.id.split("-")[1];
            var name = $("#" + element.id.split("-")[1]).text();
            var categorySkill = category;
            var idheader = $("#" + id).parent().parent()[0].id;
            var currentCount = parseInt($("#" + idheader + "count").html().replace("(", "").replace(")", ""));
            var currentCountAll = parseInt($("#allcatcount").html().replace("(", "").replace(")", ""));
            currentCount--;
            currentCountAll--;
            $("#" + idheader + "count").html("(" + currentCount + ")");
            $("#allcatcount").html("(" + currentCountAll + ")");
        }
    }
}





  // mostrar sección empresa


  $("input[type=checkbox]").click(function() {
    if($("#soyempresa").is(':checked')) {

		 $("#infoemp").show("slow");

    } else {
	 $("#infoemp").hide("slow");

    }
  });


//columna oculta 

$(".cogg").mouseenter(function () {
    $("#columnahide").css("display", "block");
    $("#columna").addClass("col-lg-10");
});

$(".cogg").mouseleave(function () {
    $("#columnahide").css("display", "none");
    $("#columna").removeClass("col-lg-10");
});

$("#habilidades").ready(function () {
    $(".skills_selected>li").css("display", "inline-block");
});




// codigo puesto por juan fdo castro para el manejo de popups divs
function pop(div) {
    document.getElementById(div).style.display = 'block';
}
function hide(div) {
    document.getElementById(div).style.display = 'none';
}
//To detect escape button
document.onkeydown = function (evt) {
    evt = evt || window.event;
    if (evt.keyCode == 27) {
        hide('popDiv');
    }
};


//OFERTAS


//Tabulador fases

$("#fases_1").click(function () {
    $(".btn1").addClass("fase_activ");
    $(".btn2, .btn3, .btn4").removeClass("fase_activ");

    var NumFases = $(".fase_activ").val();
    var TotalFijo = $("#valorproy").val();

    $("#fase2, #fase3, #fase4").hide("slow");

    if (TotalFijo != '0') {
        document.getElementById("valfase1").value = (TotalFijo / NumFases).toFixed();
        document.getElementById("valfase2").value = (TotalFijo / NumFases).toFixed();
        document.getElementById("valfase3").value = (TotalFijo / NumFases).toFixed();
        document.getElementById("valfase4").value = (TotalFijo / NumFases).toFixed();

        document.getElementById("valfase1-s").value = (100 / NumFases);
        document.getElementById("valfase2-s").value = (100 / NumFases);
        document.getElementById("valfase3-s").value = (100 / NumFases);
        document.getElementById("valfase4-s").value = (100 / NumFases);
    }
    //clear_form_elements("fase2")
    //clear_form_elements("fase3")
    //clear_form_elements("fase4")
});



$("#fases_2").click(function () {
    $(".btn2").addClass("fase_activ");
    $(".btn1, .btn3, .btn4").removeClass("fase_activ");

    var NumFases = $(".fase_activ").val();
    var TotalFijo = $("#valorproy").val();

    $("#fase2").show("slow");
    $("#fase3, #fase4").hide("slow");

    if (TotalFijo != '0') {

        document.getElementById("valfase1").value = (TotalFijo / NumFases).toFixed();
        document.getElementById("valfase2").value = (TotalFijo / NumFases).toFixed();
        document.getElementById("valfase3").value = (TotalFijo / NumFases).toFixed();
        document.getElementById("valfase4").value = (TotalFijo / NumFases).toFixed();

        document.getElementById("valfase1-s").value = (100 / NumFases);
        document.getElementById("valfase2-s").value = (100 / NumFases);
        document.getElementById("valfase3-s").value = (100 / NumFases);
        document.getElementById("valfase4-s").value = (100 / NumFases);
    }
    //clear_form_elements("fase3")
    //clear_form_elements("fase4")
    

});



$("#fases_3").click(function () {
    $(".btn3").addClass("fase_activ");
    $(".btn1, .btn2, .btn4").removeClass("fase_activ");
    var NumFases = $(".fase_activ").val();
    var TotalFijo = $("#valorproy").val();

    $("#fase2, #fase3").show("slow");
    $("#fase4").hide("slow");

    if (TotalFijo != '0') {

        document.getElementById("valfase1").value = (TotalFijo / NumFases).toFixed();
        document.getElementById("valfase2").value = (TotalFijo / NumFases).toFixed();
        document.getElementById("valfase3").value = (TotalFijo / NumFases).toFixed();
        document.getElementById("valfase4").value = (TotalFijo / NumFases).toFixed();

        document.getElementById("valfase1-s").value = (100 / NumFases);
        document.getElementById("valfase2-s").value = (100 / NumFases);
        document.getElementById("valfase3-s").value = (100 / NumFases);
        document.getElementById("valfase4-s").value = (100 / NumFases);
    }

    //clear_form_elements("fase4")
   
});



$("#fases_4").click(function () {

    $(".btn4").addClass("fase_activ");
    $(".btn1, .btn2, .btn3").removeClass("fase_activ");

    var NumFases = $(".fase_activ").val();
    var TotalFijo = $("#valorproy").val();

    $("#fase2, #fase3, #fase4").show("slow");

    if (TotalFijo != '0') {

        document.getElementById("valfase1").value = (TotalFijo / NumFases).toFixed();
        document.getElementById("valfase2").value = (TotalFijo / NumFases).toFixed();
        document.getElementById("valfase3").value = (TotalFijo / NumFases).toFixed();
        document.getElementById("valfase4").value = (TotalFijo / NumFases).toFixed();

        document.getElementById("valfase1-s").value = (100 / NumFases);
        document.getElementById("valfase2-s").value = (100 / NumFases);
        document.getElementById("valfase3-s").value = (100 / NumFases);
        document.getElementById("valfase4-s").value = (100 / NumFases);
    }


});


//descripción subasta
$("#info_subicon").click(function () {
    $("#info_sub").fadeToggle();

});

//descripción fases



$("#info_fasesicon").click(function () {
    $("#info_fases").fadeToggle();

});

//descripción tipo de valor

$("#tipo_valicon").click(function () {
    $("#tipo_val").fadeToggle();

});

//auto-val

$("#auto").click(function () {
    var NumFases = $(".fase_activ").val();
    var TotalFijo = $("#valorproy").val();

    if (TotalFijo != '0') {

        $(".valfijo input, .valsubasta input").attr('readonly', true);
        $(".valfijo input, .valsubasta input").css('color', '#666');
        $(this).addClass("btnchosen");
        $("#manual").removeClass("btnchosen");
        document.getElementById("valfase1").value = (TotalFijo / NumFases).toFixed();
        document.getElementById("valfase2").value = (TotalFijo / NumFases).toFixed();
        document.getElementById("valfase3").value = (TotalFijo / NumFases).toFixed();
        document.getElementById("valfase4").value = (TotalFijo / NumFases).toFixed();

        document.getElementById("valfase1-s").value = (100 / NumFases);
        document.getElementById("valfase2-s").value = (100 / NumFases);
        document.getElementById("valfase3-s").value = (100 / NumFases);
        document.getElementById("valfase4-s").value = (100 / NumFases);

    }

    else {
        alert('selecciona un valor para calcular');
    }
});

$("#manual").click(function () {

    var TotalFijo = $("#valorproy").val();

    if (TotalFijo != '0') {

        $(".valfijo input, .valsubasta input").attr('readonly', false);
        $(".valfijo input, .valsubasta input").css('color', '#000');
        $(this).addClass("btnchosen");
        $("#auto").removeClass("btnchosen");

    }

    else {
        alert('Primero selecciona un valor total del proyecto');
    }



});


//ampliar ofertas

$(".btn-wide").click(function () {
    $('#newoffer' + this.id).children(".widing").show("slow");
    $('#newoffer' + this.id).children(".pie-btn").hide("slow");

});

$(".colapsa").click(function () {
    $('#newoffer' + this.id).children(".widing").hide("slow");
    $('#newoffer' + this.id).children(".pie-btn").show("slow");

});

//PROYECTOS

//tabulador

$("#applied-btn").click(function () {
    $(this).addClass("project-tab-selected");
    $("#incourse-btn, #finished-btn, #deleted-btn").removeClass("project-tab-selected");
    $("#applied").show("slow");
    $("#incourse, #finished, #deleted").hide("slow");
});

$("#incourse-btn").click(function () {
    $(this).addClass("project-tab-selected");
    $("#applied-btn, #finished-btn, #deleted-btn").removeClass("project-tab-selected");
    $("#incourse").show("slow");
    $("#applied, #finished, #deleted").hide("slow");
});

$("#finished-btn").click(function () {
    $(this).addClass("project-tab-selected");
    $("#incourse-btn, #applied-btn, #deleted-btn").removeClass("project-tab-selected");
    $("#finished").show("slow");
    $("#applied, #incourse, #deleted").hide("slow");
});

$("#deleted-btn").click(function () {
    $(this).addClass("project-tab-selected");
    $("#incourse-btn, #finished-btn, #applied-btn").removeClass("project-tab-selected");
    $("#deleted").show("slow");
    $("#applied, #incourse, #finished").hide("slow");
});

function clear_form_elements(class_name) {
    jQuery("." + class_name).find(':input').each(function () {
        switch (this.type) {
            case 'password':
            case 'text':
            case 'textarea':
            case 'file':
            case 'select-one':
            case 'select-multiple':
                jQuery(this).val('');
                break;
            case 'checkbox':
            case 'radio':
                this.checked = false;
        }
    });
}












