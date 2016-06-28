WebFontConfig = {
	google: {
		families: ['Open+Sans:400,300,400italic,300italic,600,600italic,700,700italic:latin']
	}
};
(function() {
	var wf = document.createElement('script');
	wf.src = ('https:' == document.location.protocol ? 'https' : 'http') +
		'://ajax.googleapis.com/ajax/libs/webfont/1/webfont.js';
	wf.type = 'text/javascript';
	wf.async = 'true';
	var s = document.getElementsByTagName('script')[0];
	s.parentNode.insertBefore(wf, s);
})();



 $(document).ready(function () {           
	$('.menu-normal-icon').click(function () {            
		$('#sidebar').addClass('menu-min');
	})
	$('.menu-min-icon').click(function () {            
		$('#sidebar').removeClass('menu-min');
	})
	
  })
  
  
  $(document).ready(function() {
    $('#showmenu').click(function() {
    var hidden = $('.sidebar-a-menu').data('hidden');
    if(hidden){
        $('.sidebar-a-menu').animate({
            left: '-100%'
        },700)
    } else {
        $('.sidebar-a-menu').animate({
            left: '0px'
        },700)
    }
    $('.sidebar-a-menu,.image').data("hidden", !hidden);

    });
}); 


! function(e) {
    "use strict";
    var t;
    e.fn.tuxedoMenu = function(i) {
        var r = this;
        return t = e.extend({
            triggerSelector: ".tuxedo-menu-trigger",
            menuSelector: ".tuxedo-menu",
            isFixed: !0
        }, i), r.addClass("tuxedo-menu tuxedo-menu-pristine animated").toggleClass("tuxedo-menu-visible", !t.isFixed).toggleClass("tuxedo-menu-fixed slideOutLeft", t.isFixed), e(t.triggerSelector).addClass("tuxedo-menu-trigger"), e(t.triggerSelector).on("click", function() {
            return t.isFixed ? (e(t.menuSelector).toggleClass("slideInLeft slideOutLeft").addClass("tuxedo-menu-visible"), !1) : void 0
        }), e(document).click(function(i) {
            t.isFixed && !e(i.target).is(t.triggerSelector) && (e(i.target).is(t.triggerSelector) || e(i.target).closest(t.menuSelector).length || e(t.menuSelector).removeClass("slideInLeft").addClass("slideOutLeft"))
        }), r
    }
}(jQuery);

(function($) {
    var isFixed = true;
    $('#sidebar-1').tuxedoMenu({
        isFixed: isFixed
    });
    $('#toggle-is-fixed').on('click', function() {
        $('#sidebar-1').tuxedoMenu({
            isFixed: isFixed = !isFixed
        });
        $('#menu-container').toggleClass('col-sm-3');
        $('.tuxedo-menu-trigger').toggleClass('hidden');
        $(this).html(isFixed ? 'Drawer' : 'sidebar-1');
    });
})(jQuery);


//Hide SIde Menu START
 $(document).ready(function () {           
	$('.menu-normal-icon').click(function () {            
		$('#sidebar').addClass('menu-min');
	})
	$('.menu-min-icon').click(function () {            
		$('#sidebar').removeClass('menu-min');
	})
	
 })
  //Hide SIde Menu CLOSE
  
//Hide SIde Menu START
 $(document).ready(function () {           
	$('.menu-trigger').click(function () {            
		$('#sidebar').addClass('show-menu');
	})
	
	$('.menu-min-icon').click(function () {            
		$('#sidebar').removeClass('menu-min');
	})
	
})
  //Hide SIde Menu CLOSE
  
  

  
//Tooltip START

$(document).ready(function(){
    $('[data-toggle="tooltip"]').tooltip(); 
});

//Tooltip CLOSE


//File upload JS Start

! function(e) {
    var t = function(t, n) {
        this.$element = e(t), this.type = this.$element.data("uploadtype") || (this.$element.find(".thumbnail").length > 0 ? "image" : "file"), this.$input = this.$element.find(":file");
        if (this.$input.length === 0) return;
        this.name = this.$input.attr("name") || n.name, this.$hidden = this.$element.find('input[type=hidden][name="' + this.name + '"]'), this.$hidden.length === 0 && (this.$hidden = e('<input type="hidden" />'), this.$element.prepend(this.$hidden)), this.$preview = this.$element.find(".fileupload-preview");
        var r = this.$preview.css("height");
        this.$preview.css("display") != "inline" && r != "0px" && r != "none" && this.$preview.css("line-height", r), this.original = {
            exists: this.$element.hasClass("fileupload-exists"),
            preview: this.$preview.html(),
            hiddenVal: this.$hidden.val()
        }, this.$remove = this.$element.find('[data-dismiss="fileupload"]'), this.$element.find('[data-trigger="fileupload"]').on("click.fileupload", e.proxy(this.trigger, this)), this.listen()
    };
    t.prototype = {
        listen: function() {
            this.$input.on("change.fileupload", e.proxy(this.change, this)), e(this.$input[0].form).on("reset.fileupload", e.proxy(this.reset, this)), this.$remove && this.$remove.on("click.fileupload", e.proxy(this.clear, this))
        },
        change: function(e, t) {
            if (t === "clear") return;
            var n = e.target.files !== undefined ? e.target.files[0] : e.target.value ? {
                name: e.target.value.replace(/^.+\\/, "")
            } : null;
            if (!n) {
                this.clear();
                return
            }
            this.$hidden.val(""), this.$hidden.attr("name", ""), this.$input.attr("name", this.name);
            if (this.type === "image" && this.$preview.length > 0 && (typeof n.type != "undefined" ? n.type.match("image.*") : n.name.match(/\.(gif|png|jpe?g)$/i)) && typeof FileReader != "undefined") {
                var r = new FileReader,
                    i = this.$preview,
                    s = this.$element;
                r.onload = function(e) {
                    i.html('<img src="' + e.target.result + '" ' + (i.css("max-height") != "none" ? 'style="max-height: ' + i.css("max-height") + ';"' : "") + " />"), s.addClass("fileupload-exists").removeClass("fileupload-new")
                }, r.readAsDataURL(n)
            } else this.$preview.text(n.name), this.$element.addClass("fileupload-exists").removeClass("fileupload-new")
        },
        clear: function(e) {
            this.$hidden.val(""), this.$hidden.attr("name", this.name), this.$input.attr("name", "");
            if (navigator.userAgent.match(/msie/i)) {
                var t = this.$input.clone(!0);
                this.$input.after(t), this.$input.remove(), this.$input = t
            } else this.$input.val("");
            this.$preview.html(""), this.$element.addClass("fileupload-new").removeClass("fileupload-exists"), e && (this.$input.trigger("change", ["clear"]), e.preventDefault())
        },
        reset: function(e) {
            this.clear(), this.$hidden.val(this.original.hiddenVal), this.$preview.html(this.original.preview), this.original.exists ? this.$element.addClass("fileupload-exists").removeClass("fileupload-new") : this.$element.addClass("fileupload-new").removeClass("fileupload-exists")
        },
        trigger: function(e) {
            this.$input.trigger("click"), e.preventDefault()
        }
    }, e.fn.fileupload = function(n) {
        return this.each(function() {
            var r = e(this),
                i = r.data("fileupload");
            i || r.data("fileupload", i = new t(this, n)), typeof n == "string" && i[n]()
        })
    }, e.fn.fileupload.Constructor = t, e(document).on("click.fileupload.data-api", '[data-provides="fileupload"]', function(t) {
        var n = e(this);
        if (n.data("fileupload")) return;
        n.fileupload(n.data());
        var r = e(t.target).closest('[data-dismiss="fileupload"],[data-trigger="fileupload"]');
        r.length > 0 && (r.trigger("click.fileupload"), t.preventDefault())
    })
}(window.jQuery)

//File upload JS Close