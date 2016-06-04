function SRA_XHR(page) {
	var xhr = new XMLHttpRequest();
	xhr.addEventListener("load", function() {});
	xhr.open("GET", "http://localhost:56037/" + page)
	xhr.send();
}

document.onreadystatechange = function() {
	if (document.readyState != "complete") return; // Wait for the links to actually be there
	document.getElementById("start").onclick = function() {SRA_XHR("Start.aspx");}
	document.getElementById("stop").onclick = function() {SRA_XHR("Stop.aspx");}
	console.log("Handlers installed");
}