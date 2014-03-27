using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuickStart
{
    public class EventLogConsole : WebControl
    {
        private const string ClientScript =
            @"function clearLog()
            {{
	            var log = $telerik.$('#{0} div.qsfConsole');
	
	            log.empty();
            }}

            function logEvent(text, msg)
            {{
                var log = $telerik.$('#{0} div.qsfConsole')
    
                if(msg == 'info')
	                $telerik.$('<span id=info>'+text+'</span>').appendTo(log);
                else
	                $telerik.$('<span id=error>'+text+'</span>').appendTo(log);
	            log.scrollTop( 1000000 );
            }}";

                    private const string ScrollScript =
                        @"$telerik.$(document).ready(function()
	            {{
		            $telerik.$('#{0} div.qsfConsole').scrollTop(1000000);
	            }}
            );";

        public IList<string> LoggedEvents
        {
            get { return (IList<string>)(ViewState["LoggedEvents"] ?? (ViewState["LoggedEvents"] = new List<string>())); }
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            CssClass = string.Join(" ", new string[] { "qsfEventLogWrapper", CssClass });

            base.AddAttributesToRender(writer);
        }

        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Div; }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (AllowClear)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:clearLog()");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "qsfConsoleClear");
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.WriteLine("Clear log");
                writer.RenderEndTag();
            }
            writer.WriteLine(Title);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "qsfConsole");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            foreach (string loggedEvent in LoggedEvents)
            {
                //writer.RenderBeginTag(HtmlTextWriterTag.Span);
                string[] log = loggedEvent.Split('|');
                writer.WriteLine("<span id={0}>", log[1]);
                writer.WriteLine(log[0]);
                writer.WriteLine("</span>");
                //writer.RenderEndTag();
            }

            writer.RenderEndTag();
        }

        [DefaultValue(false)]
        public bool AllowClear
        {
            get { return (bool)(ViewState["AllowClear"] ?? false); }
            set { ViewState["AllowClear"] = value; }
        }

        private string title = "Event log:";

        [DefaultValue("Event log:")]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "console-main-script",
                string.Format(ClientScript, ClientID),
                true
                );
            ScriptManager.RegisterStartupScript(this, GetType(),
                "console-scroll-script",
                string.Format(ScrollScript, ClientID),
                true
                );
        }
    }
}