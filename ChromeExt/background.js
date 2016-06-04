chrome.tabs.onUpdated.addListener(function(tabId, changeInfo, tab) {
	if (changeInfo.url != null && tab.active) {
		if (changeInfo.url.indexOf("/review/") == -1) return;
		var xhr = new XMLHttpRequest();
		xhr.addEventListener("load", function() {});
		xhr.open("GET", "http://localhost:56037/Add.aspx?page=" + tab.url)
		xhr.send();
		console.log("Added " + tab.url);
	}
})