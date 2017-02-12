//Exit in case of a process hang
setTimeout(function() {
  phantom.exit();
}, 150000);

var fs = require('fs');
var page = require('webpage').create();
page.open("http://slither.io", function(status) {
  if (status === "success") {
    page.onConsoleMessage = function(msg, lineNum, sourceId) {
      console.log('CONSOLE: ' + msg);
      var stream = fs.open('../Assets/BotNames/names.txt', 'a');
      stream.writeLine(msg);
      stream.close();
    };
    page.evaluate(function() {
      document.getElementsByClassName("sadd1")[0].click()
      // page is redirecting.
    });
    setTimeout(function() {
      page.evaluate(function() {
        names = document.getElementsByClassName('nsi')[17].getElementsByTagName("span");
        for (i = 0; i < names.length; i++) {
        	console.log(names[i].innerText);
        }
      });
      phantom.exit();
    }, 50000);
  }
});