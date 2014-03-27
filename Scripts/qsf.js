// top search

function doSearch(sender, args)
{
	window.location.href = args.get_item().get_value();
}

function clearSearch()
{
	var searchBox = document.getElementById("searchBox");
	if (searchBox.value == "Search examples")
	{
		searchBox.value = "";
	}
}


// expand, collapse examples

function toggleDisplay(blockId, titleId)
{
	var block = document.getElementById(blockId);
	var title = document.getElementById(titleId);
	if (block.className.indexOf("qsfNone") == -1)
	{
		block.className = block.className + " qsfNone";
		title.className = "qsfSubtitleCollapsed";
	}
	else
	{
		block.className = block.className.replace(/qsfNone/, "");
		title.className = "qsfSubtitle";
	}
	return false;
}

// Customer Skins thumb container width adjuster

function setThumbTableWidth()
{
	var skinChooserTable = $get(SkinChooserTableID);
	if (skinChooserTable.clientWidth != qsfClientSkinsWidth)
	{
		qsfClientSkinsWidth = skinChooserTable.clientWidth;
		var qsfClientSkinsTable = $get("qsfClientSkins");
		qsfClientSkinsTable.style.width = qsfClientSkinsWidth + "px";
		$get(CustomerSkinsThumbID).style.zoom = 1;
		qsfClientSkinsTable.style.zoom = 1;
		window.setTimeout("setThumbTableWidth()", 2000);
	}
}

// demo thumbnails slider

function InitDemoSlider(sender, eventArgs)
{
	var sliderId = sender.get_id();
	eval("initialValue_" + sliderId + " = sender.get_minimumValue() + 10;");
	
	HandleClientValueChange(sender, null);
}

function HandleClientValueChange(sender, eventArgs)
{
	var sliderId = sender.get_id();
	
	var wrapper = document.getElementById(sliderId + "_wrapper");
	var content = document.getElementById(sliderId + "_content");

	var oldValue = (eventArgs) ? eventArgs.get_oldValue() : (sender.get_minimumValue() + 10);
	var change = sender.get_value() - oldValue;

	var contentWidth = content.scrollWidth - wrapper.offsetWidth;
	var calculatedChangeStep = contentWidth / (((sender.get_maximumValue() - 10) - (sender.get_minimumValue() + 10)) / sender.get_slideStep());

	eval("initialValue_" + sliderId + " = initialValue_" + sliderId + " - change * calculatedChangeStep;");

	eval("content.style.left = initialValue_" + sliderId + " + 'px';");
	
	if (sender.get_value() == (sender.get_minimumValue() + 10))
	{
		eval("content.style.left = 0;");
	}
}

function OnClientValueChanging(sender, args)
{
    // In order for the dragHandle of the slider to never leave the track, 
    // the values from -1 to -10 and from 101 and 110 are not valid values.
    var newValue = args.get_newValue();
    if(newValue < 0)
    {
        args.set_cancel(true);
        sender.set_value(0);
    }
    else if(newValue > 100)
    {
        args.set_cancel(true);
        sender.set_value(100);
    }
}

function slideConfig(uniqudId, dir)
{
	if (!uniqudId) { return; }
	
    var opposite = (dir == 'Down' ? 'Up' : 'Down');
    
    $telerik.$("#" + uniqudId + " .cfgContent")["slide" + dir](300);
    $telerik.$("#" + uniqudId + " .cfgHead span.cfgButton")
		.removeClass("cfgUp")
		.removeClass("cfgDown")
		.addClass("cfg" + opposite);
		
    $telerik.$("#" + uniqudId + " .cfgHead")[0].href = "javascript:slideConfig('" + uniqudId + "','" + opposite + "');";
    $telerik.$("input[name='" + uniqudId + "']").val(dir == "Down" ? "true" : "false");
}

function openInNewWindow(a)
{
	window.open(a.href, "_blank");	
	return false;
}

window.onload =  (function () {
	var resetNotice = $get('qsfDbResetNotice');
	
	if (resetNotice) {
		var timeout = resetNotice.getElementsByTagName("strong")[0];


		
		var remainingTime = {
			h: /(\d+) hour/gi,
			m: /(\d+) minute/gi,
			s: /(\d+) second/gi
		};
		
		var initialValue = timeout.firstChild.nodeValue;


		
		remainingTime.h = remainingTime.h.exec(initialValue);
		remainingTime.m = remainingTime.m.exec(initialValue);
		remainingTime.s = remainingTime.s.exec(initialValue);
		
		for (var i in remainingTime) { remainingTime[i] = remainingTime[i] ? remainingTime[i][1] : 0 }

		
		var tickInterval = null;

		
		var tick = function () {
		
			var timeFormatter = [];
			
			--remainingTime.s;
			
			if (remainingTime.s < 0) {
				--remainingTime.m;
				
				if (remainingTime.m < 0) {
					--remainingTime.h;
					
					if (remainingTime.h < 0) {
						clearInterval (tickInterval);
						window.location.href = window.location.href;
						return;
					}
					
					remainingTime.m = 59;
				}
					
				remainingTime.s = 59;








			}
			
			if (remainingTime.h > 0) {
				timeFormatter[timeFormatter.length] = remainingTime.h;
				timeFormatter[timeFormatter.length] = remainingTime.h > 1 ? " hours, " : " hour, ";
			}
			
			if (remainingTime.m > 0) {
				timeFormatter[timeFormatter.length] = remainingTime.m;
				timeFormatter[timeFormatter.length] = remainingTime.m > 1 ? " minutes, " : " minute, ";
			}
					
			timeFormatter[timeFormatter.length] = remainingTime.s;
			timeFormatter[timeFormatter.length] = remainingTime.s > 1 ? " seconds" : " second";
			
			timeout.innerHTML = timeFormatter.join("");
		};
		
		tickInterval = setInterval (tick, 1000);


	}
});


var isChooserInitialized = false;

function openSkinChooser(e) {
    if (isChooserInitialized)
        return;
   
   $telerik.cancelRawEvent(e);
        
   isChooserInitialized = true;
   initializeSkinChooser();

    var $ = $telerik.$;
    $('.qsfSkinMgr').click();
}

function initializeSkinChooser() 
{
    var $ = $telerik.$;
    var topWrapper = $(".rateMenu");

    $('.qscDropDownList a', topWrapper).bind('click', function (e) {
        e.stopImmediatePropagation();
        e.preventDefault();
        
        var link = $(this);
        
        if (!link.hasClass("qscSelected")) {
            link.addClass("qscSelected");
            topWrapper.css("z-index", 50000);
        }

		// Clear selection
		if (document.selection && document.selection.empty)
			document.selection.empty();
		else if (window.getSelection && window.getSelection().removeAllRanges)
			window.getSelection().removeAllRanges();
    });
    
}

// QSF demos rating and feedback
var qsfRatingControl = null;
function OnClientQSFDemoLoad(sender, args)
{
    qsfRatingControl = sender;
}

function GetClientQSFDemoRating(control)
{
    if(qsfRatingControl)
        return qsfRatingControl.get_value().toString();
    
    return "0";
}

function OnClientQSFDemoRated(sender, args)
{
    var demoRating = GetClientQSFDemoRating();
    if(0 < parseFloat(demoRating))
    {
        var params = {rating: demoRating,
                      demo: qsfCurrentDemo};

        sender.set_readOnly(true);
        Sys.Net.WebServiceProxy.invoke(qsfDemoWebServicePath, "SaveRating", false, { context: params }, 
            Function.createDelegate(sender, function()
            {
                this.get_element().title = "You have already rated this demo. Thank you for your feedback.";
            }),
            OnWebServiceQSFDemoRateError);
    }
}

function OnWebServiceQSFDemoRateError(error)
{
    alert(error.get_message());
    if(qsfRatingControl)
    {
        qsfRatingControl.set_readOnly(false);
    }
}

function GetQSFCommentControl()
{
	return (window.QSFCommentControl ? QSFCommentControl : null);
}

function GetQSFCommentText()
{
	var comment = GetQSFCommentControl();
    return comment ? comment.value : "";
}

function ClearQSFCommentText()
{
	var comment = GetQSFCommentControl();
	if(comment)
		comment.value = "";
}

function RepaintQSFRatingControl()
{
	if(qsfRatingControl)
		qsfRatingControl.repaint();
}

function PostQSFDemoComment()
{
    var text = GetQSFCommentText();
    if("" != text.replace(/^\s\s*/, "").replace(/\s\s*$/, ""))
    {
        var params = {comment: text, demo: qsfCurrentDemo};

        Sys.Net.WebServiceProxy.invoke(qsfDemoWebServicePath, "SaveComment", false, { context: params }, 
            ClearQSFCommentText,
            OnWebServiceQSFDemoRateError);
    }
    else
    {
        ClearQSFCommentText();
    }

    return false;
}

var isAllProdsInitialized = false;

function openAllProds(e) {
    if (isAllProdsInitialized)
		return false;
	
	$telerik.cancelRawEvent(e);
	
	isAllProdsInitialized = true;
	initializeAllProds();
	
	$telerik.$(".suiteAll a").click();
}

function initializeAllProds() 
{
	var allProdsLink = $telerik.$(".suiteAll a");
    var allProdsCnt = $telerik.$("#qsfAllProds");
    var allProdsDrop = $telerik.$('.qsfAllProdsDrop', allProdsCnt);
	
	allProdsDrop.before("<iframe style='border:0;'></iframe>");
	
    allProdsCnt.css({visibility: "hidden", display: "block"});
    var allProdsDropHeight = allProdsDrop[0].offsetHeight + ($telerik.isIE7 ? 10 : 0);
    allProdsCnt.css({visibility: "visible", display: "none", height: allProdsDropHeight + "px"});
    allProdsDrop.css({height: allProdsDropHeight + "px", top: -allProdsDropHeight});
    
    var closeAllProdsDrop = function()
    {
		allProdsDrop.stop().animate(
			{ top: -allProdsDropHeight + "px" },
			{	duration: 300,
				complete: function() {
					allProdsCnt.hide();
					allProdsLink.removeClass("allActive");
				}
			});
    }
    
    // Close skin chooser on document click
    $telerik.$().bind("click", closeAllProdsDrop);
    
    // Handle click on the 
    $telerik.$(".suiteAll").bind("click", function (e) {
        if (!allProdsLink.is(".allActive")) {
            allProdsLink.addClass("allActive");
            allProdsCnt.show();
            allProdsDrop.stop().animate( { top: 0 }, 300 );
        }
        else {
            closeAllProdsDrop();
		}

		// Clear selection
		if (document.selection && document.selection.empty)
			document.selection.empty();
		else if (window.getSelection && window.getSelection().removeAllRanges)
			window.getSelection().removeAllRanges();

		e.stopPropagation(); e.preventDefault();
    });
    
    allProdsCnt.click(function(e) {
		if ($telerik.$(e.target).is("div, ul, li, strong"))
		{
			e.preventDefault(); e.stopPropagation();
		}
	});
}

function qsfSearchDropDownOpening(sender)
{
    var parent = $telerik.$(sender.get_element().parentNode);

    if (!parent.hasClass('qsfSearchOpened'))
        parent.addClass('qsfSearchOpened');
}

function qsfSearchDropDownClosed(sender)
{
    var parent = $telerik.$(sender.get_element().parentNode);

    if (parent.hasClass('qsfSearchOpened'))
        setTimeout(function () { parent.removeClass('qsfSearchOpened') }, 350); // This should be fixed in RadComboBox.
}
