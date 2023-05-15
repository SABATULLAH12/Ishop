/* jquery slider pips plugin, version 1.1 */

(function ($) {

    var extensionMethods = {

        pips: function (settings) {

            var options = {

                first: "label", // "pip" , false
                last: "label", // "pip" , false
                rest: "pip", // "label" , false
                labels: false,	 // [array]
                prefix: "",	 // "", string
                suffix: ""	 // "", string

            };

            $.extend(options, settings);

            // labels are needed globally, potentially.
            this.options.labels = options.labels

            // get rid of all pips that might already exist.
            this.element.addClass('ui-slider-pips').find('.ui-slider-pip').remove();

            // we need the amount of pips to create.
            var pips = (this.options.max - this.options.min) / this.options.step;

            // for every stop in the slider; we create a pip.
            for (i = 0; i <= pips; i++) {

                // create the label name, it's either the item in the array, or a number.
                var label = (this.options.labels) ? this.options.labels[i] : (this.options.min + (this.options.step * i));
                if (typeof (label) === "undefined") { label = ""; }


                // hold a span element for the pip
                var s = $('<span class="ui-slider-pip ui-slider-pip-' + i + '"><span class="ui-slider-line"></span><span class="ui-slider-label">' + options.prefix + label + options.suffix + '</span></span>');

                // add a class so css can handle the display
                // we'll hide labels by default in CSS, and show them if set.
                // we'll also use CSS to hide the pip altogether.
                if (0 == i) {
                    s.addClass('ui-slider-pip-first');
                    if ("label" == options.first) { s.addClass('ui-slider-pip-label'); }
                    if (false == options.first) { s.addClass('ui-slider-pip-hide'); }
                } else if (pips == i) {
                    s.addClass('ui-slider-pip-last');
                    if ("label" == options.last) { s.addClass('ui-slider-pip-label'); }
                    if (false == options.last) { s.addClass('ui-slider-pip-hide'); }
                } else {
                    if ("label" == options.rest) { s.addClass('ui-slider-pip-label'); }
                    if (false == options.rest) { s.addClass('ui-slider-pip-hide'); }
                }


                // if it's a horizontal slider we'll set the left offset,
                // and the top if it's vertical.
                if (this.options.orientation == "horizontal")
                    s.css({ left: '' + (100 / pips) * i + '%' });
                else
                    s.css({ top: '' + (100 / pips) * i + '%' });


                // append the span to the slider.
                this.element.append(s);

            }

        }


    };

    $.extend(true, $['ui']['slider'].prototype, extensionMethods);


})(jQuery);



/* jquery slider float plugin, version 1.1 */

(function ($) {

    var extensionMethods = {

        float: function (settings) {

            var options = {
                handle: true, // false
                labels: true,	 // false
                prefix: "",	 // "", string
                suffix: ""	 // "", string
            };
            $.extend(options, settings);

            // add a class for the CSS
            this.element.addClass('ui-slider-float');


            // apply handle tip if we settings allows.
            if (options.handle) {

                // if this is a range slider
                if (this.options.values) {

                    var $tip = [
                        $('<span class="ui-slider-tip">' + $(".ui-slider-pips .ui-slider-pip-" + this.options.values[0] + "").children(".ui-slider-label").html() + '</span>'),
                        $('<span class="ui-slider-tip">' + $(".ui-slider-pips .ui-slider-pip-" + this.options.values[1] + "").children(".ui-slider-label").html() + '</span>')
                    ];
                   
                   
                  
                    // else if its just a normal slider
                } else {

                    // create a tip element
                    var $tip = $('<span class="ui-slider-tip">' + $(".ui-slider-pips .ui-slider-pip-" + this.options.value + "").children(".ui-slider-label").html() + '</span>');
                    //$("#slidervalue").html($(".ui-slider-pips .ui-slider-pip-" + this.options.value + "").children(".ui-slider-label").html());
                    isChange = "true";
                    //Custom Region changes added by Bramhanath(06-01-2016)
                    //if ($(".TableImgContentSelector").css('background-color') == "rgb(72, 99, 112)" || $(".ImgContentSelectorPro").css('background-color') == "rgb(72, 99, 112)") {
                    CheckTimePeriodChanged = "Yes";
                    SelectGeographyFilter('', '', '', '', '', '', '', '');
                    if (hideAdvanceFilterType == "Geography") {
                        //LoadTypeData(hideAdvanceFilterType, '', hideAdvanceFilterType);
                        //$("#DropDownType").show();
                        if ($(".TableImgContentSelector").css('background-color') == "rgb(72, 99, 112)" && $(".TableImgContentSelector").css('background-position') == "-181px -1034px") {
                            ShowViewContent(viewBlock, "Clear");
                        }
                        else {
                            ShowViewContent(ViewBlock, "Clear");
                        }
                        //}
                    }
                    //
                    if (currentpage == "hdn-analysis-acrosstrips") {
                    if (TimeExtension == "Total") {
                        TimePeriod = "total|total";
                        $(".timeType").html("Total Time");
                        if ($('.totimeperiod').css('display') == 'none') {
                            $('.TimePeriod_Label').html($(".timeType").val());
                            $(".TimePeriod_LabelShopping").html($(".timeType").val());
                            $(".TimePeriod_LabelPercep").html($(".timeType").val());
                            $(".TimePeriod_LabelTotal").html($(".timeType").val());
                            var timeperiodarray = [];
                            timeperiodarray = $(".timeType").val().split(" ");
                            PreviousTimePeriod = "";                          
                            for (var i = 0; i < timeperiodarray.length; i++)
                            {
                                if ($.isNumeric(timeperiodarray[i])) {
                                    timeperiodarray[i] = timeperiodarray[i] - 1;
                                }                                
                            }
                            PreviousTimePeriod = timeperiodarray.join(" ");
                            $(".TimePeriod_LabelBGM").html($(".timeType").val() + " v/s " + timeperiodarray.join(" "));
                        }
                        else {
                            $('.TimePeriod_Label').html($(".timeType").val() + ' TO ' + $($('.totimeperiod')[1]).text());
                            $(".TimePeriod_LabelShopping").html($(".timeType").val());
                            $(".TimePeriod_LabelPercep").html($(".timeType").val()); //+ ' TO ' + $($('.totimeperiod')[1]).text());
                            $(".TimePeriod_LabelTotal").html($(".timeType").val()); //+ ' TO ' + $($('.totimeperiod')[1]).text());
                            var timeperiodarray = [];
                            timeperiodarray = $(".timeType").val().split(" ");
                            PreviousTimePeriod = "";                           
                            for (var i = 0; i < timeperiodarray.length; i++) {
                                if ($.isNumeric(timeperiodarray[i])) {
                                    timeperiodarray[i] = timeperiodarray[i] - 1;
                                }                              
                            }
                            PreviousTimePeriod = timeperiodarray.join(" ");
                            $(".TimePeriod_LabelBGM").html($(".timeType").val() + " v/s " + timeperiodarray.join(" "));
                        }
                    }
                    else {
                        $(".timeType").html($(".ui-slider-pips .ui-slider-pip-" + this.options.value + "").children(".ui-slider-label").html());
                        if ($('.totimeperiod').css('display') == 'none') {
                            $('.TimePeriod_Label').html($(".timeType").val());
                            $(".TimePeriod_LabelShopping").html($(".timeType").val());
                            $(".TimePeriod_LabelPercep").html($(".timeType").val());
                            $(".TimePeriod_LabelTotal").html($(".timeType").val());
                            var timeperiodarray = [];
                            timeperiodarray = $(".timeType").val().split(" ");
                            PreviousTimePeriod = "";                            
                            for (var i = 0; i < timeperiodarray.length; i++) {
                                if ($.isNumeric(timeperiodarray[i])) {
                                    timeperiodarray[i] = timeperiodarray[i] - 1;
                                }                               
                            }
                            PreviousTimePeriod = timeperiodarray.join(" ");
                            $(".TimePeriod_LabelBGM").html($(".timeType").val() + " v/s " + timeperiodarray.join(" "));
                        }
                        else {
                            $('.TimePeriod_Label').html($(".timeType").val() + ' TO ' + $($('.totimeperiod')[1]).text());
                            $(".TimePeriod_LabelShopping").html($(".timeType").val());
                            $(".TimePeriod_LabelPercep").html($(".timeType").val()); //+ ' TO ' + $($('.totimeperiod')[1]).text());
                            $(".TimePeriod_LabelTotal").html($(".timeType").val()); //+ ' TO ' + $($('.totimeperiod')[1]).text());
                            var timeperiodarray = [];
                            timeperiodarray = $(".timeType").val().split(" ");
                            PreviousTimePeriod = "";                           
                            for (var i = 0; i < timeperiodarray.length; i++) {                              
                                if ($.isNumeric(timeperiodarray[i])) {
                                    timeperiodarray[i] = timeperiodarray[i] - 1;
                                }                              
                            }
                            PreviousTimePeriod = timeperiodarray.join(" ");
                            $(".TimePeriod_LabelBGM").html($(".timeType").val() + " v/s " + timeperiodarray.join(" "));
                        }
                        TimePeriodName = $(".ui-slider-pips .ui-slider-pip-" + this.options.value + "").children(".ui-slider-label").html().replace(" 48MMT", "").replace(" 36MMT", "").replace(" 30MMT", "").replace(" 24MMT", "").replace(" 18MMT", "").replace(" 3MMT", "").replace(" 12MMT", "").replace(" 6MMT", "");
                        TimePeriod = TimeExtension + "|" + TimePeriodName;
                    }
                }
            else{
            if (TimeExtension == "Total") {
                    TimePeriod = "total|total";
                    $(".timeType").html("Total Time");
                    if ($('.totimeperiod').css('display') == 'none') {
                        $('.TimePeriod_Label').html($(".timeType").val());
                        $(".TimePeriod_LabelShopping").html($(".timeType").val());
                        $(".TimePeriod_LabelPercep").html($(".timeType").val());
                        $(".TimePeriod_LabelTotal").html($(".timeType").val());
                        var timeperiodarray = [];
                        timeperiodarray = $(".timeType").val().split(" ");
                        PreviousTimePeriod = "";                          
                        for (var i = 0; i < timeperiodarray.length; i++)
                        {
                            if ($.isNumeric(timeperiodarray[i])) {
                                timeperiodarray[i] = timeperiodarray[i] - 1;
                            }                                
                        }
                        PreviousTimePeriod = timeperiodarray.join(" ");
                        $(".TimePeriod_LabelBGM").html($(".timeType").val() + " v/s " + timeperiodarray.join(" "));
                    }
                    else {
                        $('.TimePeriod_Label').html($(".timeType").val() + ' TO ' + $($('.totimeperiod')[1]).text());
                        $(".TimePeriod_LabelShopping").html($(".timeType").val());
                        $(".TimePeriod_LabelPercep").html($(".timeType").val()); //+ ' TO ' + $($('.totimeperiod')[1]).text());
                        $(".TimePeriod_LabelTotal").html($(".timeType").val()); //+ ' TO ' + $($('.totimeperiod')[1]).text());
                        var timeperiodarray = [];
                        timeperiodarray = $(".timeType").val().split(" ");
                        PreviousTimePeriod = "";                           
                        for (var i = 0; i < timeperiodarray.length; i++) {
                            if ($.isNumeric(timeperiodarray[i])) {
                                timeperiodarray[i] = timeperiodarray[i] - 1;
                            }                              
                        }
                        PreviousTimePeriod = timeperiodarray.join(" ");
                        $(".TimePeriod_LabelBGM").html($(".timeType").val() + " v/s " + timeperiodarray.join(" "));
                    }
                }
            else {
                        $(".timeType").html("");
                $(".timeType").html($(".ui-slider-pips .ui-slider-pip-" + this.options.value + "").children(".ui-slider-label").html());
                if ($('.totimeperiod').css('display') == 'none') {
                    $('.TimePeriod_Label').html($(".timeType").val());
                    $(".TimePeriod_LabelShopping").html($(".timeType").val());
                    $(".TimePeriod_LabelPercep").html($(".timeType").val());
                    $(".TimePeriod_LabelTotal").html($(".timeType").val());
                    var timeperiodarray = [];
                    timeperiodarray = $(".timeType").val().split(" ");
                    PreviousTimePeriod = "";                            
                    for (var i = 0; i < timeperiodarray.length; i++) {
                        if ($.isNumeric(timeperiodarray[i])) {
                            timeperiodarray[i] = timeperiodarray[i] - 1;
                        }                               
                    }
                    PreviousTimePeriod = timeperiodarray.join(" ");
                    $(".TimePeriod_LabelBGM").html($(".timeType").val() + " v/s " + timeperiodarray.join(" "));
                }
                else {
                    $('.TimePeriod_Label').html($(".timeType").val() + ' TO ' + $($('.totimeperiod')[1]).text());
                    $(".TimePeriod_LabelShopping").html($(".timeType").val());
                    $(".TimePeriod_LabelPercep").html($(".timeType").val()); //+ ' TO ' + $($('.totimeperiod')[1]).text());
                    $(".TimePeriod_LabelTotal").html($(".timeType").val()); //+ ' TO ' + $($('.totimeperiod')[1]).text());
                    var timeperiodarray = [];
                    timeperiodarray = $(".timeType").val().split(" ");
                    PreviousTimePeriod = "";                           
                    for (var i = 0; i < timeperiodarray.length; i++) {                              
                        if ($.isNumeric(timeperiodarray[i])) {
                            timeperiodarray[i] = timeperiodarray[i] - 1;
                        }                              
                    }
                    PreviousTimePeriod = timeperiodarray.join(" ");
                    $(".TimePeriod_LabelBGM").html($(".timeType").val() + " v/s " + timeperiodarray.join(" "));
                }
                TimePeriodName = $(".ui-slider-pips .ui-slider-pip-" + this.options.value + "").children(".ui-slider-label").html().replace(" 48MMT", "").replace(" 36MMT", "").replace(" 30MMT", "").replace(" 24MMT", "").replace(" 18MMT", "").replace(" 3MMT", "").replace(" 12MMT", "").replace(" 6MMT", "");
                TimePeriod = TimeExtension + "|" + TimePeriodName;
            }
                }
                    ClearOnlyData();
                }

                // now we append it to all the handles
                this.element.find('.ui-slider-handle').each(function (k, v) {
                    $(v).append($tip[k]);
                });

            }


            if (options.labels) {

                // if this slider also has labels, we'll make those into tips, too; by cloning and changing class.
                this.element.find('.ui-slider-label').each(function (k, v) {
                    var $e = $(v).clone().removeClass('ui-slider-label').addClass('ui-slider-tip-label');
                    $e.insertAfter($(v));
                });

            }

            // when slider changes, update handle tip label.
            this.element.on('slidechange slide', function (e, ui) {
                isChange = "true";
                //Custom Region changes added by Bramhanath(06-01-2016)
                //if ($(".TableImgContentSelector").css('background-color') == "rgb(72, 99, 112)" || $(".ImgContentSelectorPro").css('background-color') == "rgb(72, 99, 112)") {
                    CheckTimePeriodChanged = "Yes";
                    //SelectGeographyFilter(Value, DatabaseName, storeid, obj, type, _parentid, parentName, SingleOrMultipleSelect)
                    SelectGeographyFilter('', '', '', '', '', '', '', '');
                    if (hideAdvanceFilterType == "Geography") {
                        //LoadTypeData(hideAdvanceFilterType, '', hideAdvanceFilterType);
                        if ($(".TableImgContentSelector").css('background-color') == "rgb(72, 99, 112)" && $(".TableImgContentSelector").css('background-position') == "-181px -1034px") {
                            ShowViewContent(viewBlock, "Clear");
                        }
                        else {
                            //$("#DropDownType").show();
                            ShowViewContent(ViewBlock, "Clear");
                        }
                    //}
                }
                //
                //$(ui.handle).find('.ui-slider-tip').html(options.prefix + ui.value + options.suffix);
                $(ui.handle).find('.ui-slider-tip').html($(".ui-slider-pips .ui-slider-pip-" + ui.value + "").children(".ui-slider-label").html());
                //$("#slidervalue").html(($(".ui-slider-pips .ui-slider-pip-" + ui.value + "").children(".ui-slider-label").html()));
                
                    if (TimeExtension == "Total") {
                        TimePeriod = "total|total";
                        $(".timeType").html("Total Time");

                        if ($('.totimeperiod').css('display') == 'none') {
                            $('.TimePeriod_Label').html($(".timeType").val());
                            $(".TimePeriod_LabelShopping").html($(".timeType").val());
                            $(".TimePeriod_LabelPercep").html($(".timeType").val());
                            $(".TimePeriod_LabelTotal").html($(".timeType").val());
                            var timeperiodarray = [];
                            timeperiodarray = $(".timeType").val().split(" ");
                            PreviousTimePeriod = "";
                            for (var i = 0; i < timeperiodarray.length; i++) {
                                if ($.isNumeric(timeperiodarray[i])) {
                                    timeperiodarray[i] = timeperiodarray[i] - 1;
                                }
                            }
                            PreviousTimePeriod = timeperiodarray.join(" ");
                            $(".TimePeriod_LabelBGM").html($(".timeType").val() + " v/s " + timeperiodarray.join(" "));
                        }
                        else {
                            $('.TimePeriod_Label').html($(".timeType").val() + ' TO ' + $($('.totimeperiod')[1]).text());
                            $(".TimePeriod_LabelShopping").html($(".timeType").val());
                            $(".TimePeriod_LabelPercep").html($(".timeType").val()); //+ ' TO ' + $($('.totimeperiod')[1]).text());
                            $(".TimePeriod_LabelTotal").html($(".timeType").val()); //+ ' TO ' + $($('.totimeperiod')[1]).text());
                            var timeperiodarray = [];
                            timeperiodarray = $(".timeType").val().split(" ");
                            PreviousTimePeriod = "";
                            for (var i = 0; i < timeperiodarray.length; i++) {
                                if ($.isNumeric(timeperiodarray[i])) {
                                    timeperiodarray[i] = timeperiodarray[i] - 1;
                                }
                            }
                            PreviousTimePeriod = timeperiodarray.join(" ");
                            $(".TimePeriod_LabelBGM").html($(".timeType").val() + " v/s " + timeperiodarray.join(" "));
                        }
                        //$(".TimePeriod_Label").html($(".timeType").val());
                        
       
                    }
                    else {
                        $(".timeType").html($(".ui-slider-pips .ui-slider-pip-" + ui.value + "").children(".ui-slider-label").html());
                        if ($('.totimeperiod').css('display') == 'none') {
                            $('.TimePeriod_Label').html($(".timeType").val());
                            $(".TimePeriod_LabelShopping").html($(".timeType").val());
                            $(".TimePeriod_LabelPercep").html($(".timeType").val());
                            $(".TimePeriod_LabelTotal").html($(".timeType").val());
                            var timeperiodarray = [];
                            timeperiodarray = $(".timeType").val().split(" ");
                            PreviousTimePeriod = "";
                            for (var i = 0; i < timeperiodarray.length; i++) {
                                if ($.isNumeric(timeperiodarray[i])) {
                                    timeperiodarray[i] = timeperiodarray[i] - 1;
                                }
                            }
                            PreviousTimePeriod = timeperiodarray.join(" ");
                            $(".TimePeriod_LabelBGM").html($(".timeType").val() + " v/s " + timeperiodarray.join(" "));
                        }
                        else {
                            $('.TimePeriod_Label').html($(".timeType").val() + ' TO ' + $($('.totimeperiod')[1]).text());
                            $(".TimePeriod_LabelShopping").html($(".timeType").val());
                            $(".TimePeriod_LabelPercep").html($(".timeType").val()); //+ ' TO ' + $($('.totimeperiod')[1]).text());
                            $(".TimePeriod_LabelTotal").html($(".timeType").val()); //+ ' TO ' + $($('.totimeperiod')[1]).text());
                            var timeperiodarray = [];
                            timeperiodarray = $(".timeType").val().split(" ");
                            PreviousTimePeriod = "";
                            for (var i = 0; i < timeperiodarray.length; i++) {
                                if ($.isNumeric(timeperiodarray[i])) {
                                    timeperiodarray[i] = timeperiodarray[i] - 1;
                                }
                            }
                            PreviousTimePeriod = timeperiodarray.join(" ");
                            $(".TimePeriod_LabelBGM").html($(".timeType").val() + " v/s " + timeperiodarray.join(" "));
                        }

                        TimePeriodName = $(".ui-slider-pips .ui-slider-pip-" + ui.value + "").children(".ui-slider-label").html().replace(" 48MMT", "").replace(" 36MMT", "").replace(" 30MMT", "").replace(" 24MMT", "").replace(" 18MMT", "").replace(" 3MMT", "").replace(" 12MMT", "").replace(" 6MMT", "");
                        TimePeriod = TimeExtension + "|" + TimePeriodName;
                    }
                    ClearOnlyData();
                   
                        if ($("#showSliderTime div a").length > 0) {
                            $(".timeType").html($("#showSliderTime div a").eq(0).children(".ui-slider-tip").html());
                            if ($('.totimeperiod').css('display') == 'none') {
                                $('.TimePeriod_Label').html($(".timeType").val());
                                $(".TimePeriod_LabelShopping").html($(".timeType").val());
                                $(".TimePeriod_LabelPercep").html($(".timeType").val());
                                $(".TimePeriod_LabelTotal").html($(".timeType").val());
                                var timeperiodarray = [];
                                timeperiodarray = $(".timeType").val().split(" ");
                                PreviousTimePeriod = "";
                                for (var i = 0; i < timeperiodarray.length; i++) {
                                    if ($.isNumeric(timeperiodarray[i])) {
                                        timeperiodarray[i] = timeperiodarray[i] - 1;
                                    }
                                }
                                PreviousTimePeriod = timeperiodarray.join(" ");
                                $(".TimePeriod_LabelBGM").html($(".timeType").val() + " v/s " + timeperiodarray.join(" "));
                            }
                            else {
                                $('.TimePeriod_Label').html($(".timeType").val() + ' TO ' + $($('.totimeperiod')[1]).text());
                                $(".TimePeriod_LabelShopping").html($(".timeType").val());
                                $(".TimePeriod_LabelPercep").html($(".timeType").val()); //+ ' TO ' + $($('.totimeperiod')[1]).text());
                                $(".TimePeriod_LabelTotal").html($(".timeType").val()); //+ ' TO ' + $($('.totimeperiod')[1]).text());
                                var timeperiodarray = [];
                                timeperiodarray = $(".timeType").val().split(" ");
                                PreviousTimePeriod = "";
                                for (var i = 0; i < timeperiodarray.length; i++) {
                                    if ($.isNumeric(timeperiodarray[i])) {
                                        timeperiodarray[i] = timeperiodarray[i] - 1;
                                    }
                                }
                                PreviousTimePeriod = timeperiodarray.join(" ");
                                $(".TimePeriod_LabelBGM").html($(".timeType").val() + " v/s " + timeperiodarray.join(" "));
                            }
                        }
                        if ($("#showSliderTime div a").length >= 2) {
                            $(".totime").val($("#showSliderTime div a").eq(1).children(".ui-slider-tip").html());
                            //if ($('.totimeperiod').css('display') == 'display')
                            //    $('.TimePeriod_Label').html($(".totime").val());
                        }
                   
            });
        }
    };
    $.extend(true, $['ui']['slider'].prototype, extensionMethods);


})(jQuery);

// slider extension
(function ($) {
    if ($.ui.slider) {
        // add minimum range length option
        $.extend($.ui.slider.prototype.options, {
            minRangeSize: 0,
            maxRangeSize: 100,
            autoShift: false,
            lowMax: 100,
            topMin: 0
        });

        $.extend($.ui.slider.prototype, {
            _slide: function (event, index, newVal) {
                var otherVal,
                newValues,
                allowed,
                factor;

                if (this.options.values && this.options.values.length) {
                    otherVal = this.values(index ? 0 : 1);
                    factor = index === 0 ? 1 : -1;

                    if (this.options.values.length === 2 && this.options.range === true) {
                        // lower bound max
                        if (index === 0 && newVal > this.options.lowMax) {
                            newVal = this.options.lowMax;
                        }
                        // upper bound min
                        if (index === 1 && newVal < this.options.topMin) {
                            newVal = this.options.topMin;
                        }
                        // minimum range requirements
                        if ((otherVal - newVal) * factor < this.options.minRangeSize) {
                            newVal = otherVal - this.options.minRangeSize * factor;
                        }
                        // maximum range requirements
                        if ((otherVal - newVal) * factor > this.options.maxRangeSize) {
                            if (this.options.autoShift === true) {
                                otherVal = newVal + this.options.maxRangeSize * factor;
                            }
                            else {
                                newVal = otherVal - this.options.maxRangeSize * factor;
                            }
                        }
                    }

                    if (newVal !== this.values(index)) {
                        newValues = this.values();
                        newValues[index] = newVal;
                        newValues[index ? 0 : 1] = otherVal;
                        // A slide can be canceled by returning false from the slide callback
                        allowed = this._trigger("slide", event, {
                            handle: this.handles[index],
                            value: newVal,
                            values: newValues
                        });
                        // otherVal = this.values(index ? 0 : 1);
                        if (allowed !== false) {
                            this.values(index, newVal, true);
                            this.values((index + 1) % 2, otherVal, true);
                        }
                    }
                } else {
                    if (newVal !== this.value()) {
                        // A slide can be canceled by returning false from the slide callback
                        allowed = this._trigger("slide", event, {
                            handle: this.handles[index],
                            value: newVal
                        });
                        if (allowed !== false) {
                            this.value(newVal);
                        }
                    }
                }
            }
        });
    }
})(jQuery);
// end slider extensions