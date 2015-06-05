(function ($) {

    var Alpaca = $.alpaca;

    Alpaca.Fields.MLTextField = Alpaca.Fields.TextField.extend(
    /**
     * @lends Alpaca.Fields.TagField.prototype
     */
    {
        constructor: function (container, data, options, schema, view, connector) {
            var self = this;
            this.base(container, data, options, schema, view, connector);
            this.culture = connector.culture;
        },
        /**
         * @see Alpaca.Fields.TextField#getFieldType
         */
        getFieldType: function () {
            return "mltext";
        },

        /**
         * @see Alpaca.Fields.TextField#setup
         */
        setup: function () {

            if (this.data && Alpaca.isArray(this.data)) {             
                this.olddata = this.data;
            } else {
                this.olddata = [];
            }
            this.base();
        },
        /**
         * @see Alpaca.Fields.TextField#getValue
         */
        getValue: function () {
            var val = this.base();

            if (val === "") {
                return [];
            }
            var newData = [];
            var exist = false;
            $.each(this.olddata, function( index, value ) {
                //alert( index + ": " + value );
                var newValue;
                if (value.culture == this.culture)
                {
                    newValue = { culture: Alpaca.copyOf(value.lang), text: val };
                    exist = true;
                }
                else
                {
                    newValue = { culture : Alpaca.copyOf(value.lang),  text : Alpaca.copyOf(value.text) };
                }
                newData.push(newValue);
            });
            if (!exist)
                newData.push({ culture: this.culture, text: val });
            return newData;
        },

        /**
         * @see Alpaca.Fields.TextField#setValue
         */
        setValue: function (val) {
            if (val === "") {
                return;
            }

            if (!val) {
                this.base("");
                return;
            }
            if (Alpaca.isArray(val)) {
                var v = "";
                $.each(this.olddata, function (index, value) {
                    if (value.culture == this.culture)
                        v = value.text;
                });
                this.base(v);
            }
            else
            {
                this.base(val);
            }
        },
        afterRenderControl: function (model, callback) {
            var self = this;
            this.base(model, function () {
                self.handlePostRender(function () {
                    callback();
                });
            });
        },
        handlePostRender: function (callback) {
            var self = this;

            //var el = this.control;
            var el = this.getControlEl();

            $(this.control.get(0)).after('<img src="/images/Flags/'+this.culture+'.gif" />');
           

            callback();
        },
        
        /**
         * @see Alpaca.Fields.TextField#getTitle
         */
        getTitle: function () {
            return "Multi Language Text Field";
        },

        /**
         * @see Alpaca.Fields.TextField#getDescription
         */
        getDescription: function () {
            return "Multi Language Text field .";
        },

        /**
         * @private
         * @see Alpaca.Fields.TextField#getSchemaOfOptions
         */
        getSchemaOfOptions: function () {
            return Alpaca.merge(this.base(), {
                "properties": {
                    "separator": {
                        "title": "Separator",
                        "description": "Separator used to split tags.",
                        "type": "string",
                        "default": ","
                    }
                }
            });
        },

        /**
         * @private
         * @see Alpaca.Fields.TextField#getOptionsForOptions
         */
        getOptionsForOptions: function () {
            return Alpaca.merge(this.base(), {
                "fields": {
                    "separator": {
                        "type": "text"
                    }
                }
            });
        }

        /* end_builder_helpers */
    });

    Alpaca.registerFieldClass("mltext", Alpaca.Fields.MLTextField);

    

})(jQuery);