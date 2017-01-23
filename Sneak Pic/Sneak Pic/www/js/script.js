var gallery;
var items = [];
var isGalleryUp;
var pswpElement;
var verticalCenter = '<div style="display: table;width: 100%;height: 100%;"><div style="display: table-cell;vertical-align: middle;">';

$(document).ready(function() {
    GetEvents();
});

function GetEvents() {
  $.ajax({
      url: "http://need4bit.com/SneakPic/Events/",
      success: function(data){
        $(".feed").empty();
        aLinks = $(data).find("a");
        aLinks.each(function(){
            href = $(this).attr("href");
            if(href.indexOf('/SneakPic/Events/') != -1) {
              name = href.replace('/SneakPic/Events/','').slice(0,-1);
              $(".feed").append("<div onclick=\"GetPictures(this)\" name=\""+decodeURI(name)+"\" class=\"event greyscale\" style=\"background:url('http://need4bit.com"+href+"banner.jpg');background-size:cover;\">"+verticalCenter+decodeURI(name)+"</div></div></div>");
            }
        });
      }
    });
}

function GetPictures(selector) {
  items = [];
  imgCounter = 1;
  $.ajax({
      url: "http://need4bit.com/SneakPic/Events/" + $(selector).attr("name") + "/images/",
      success: function(data){
        $(".feed").empty();
        aLinks = $(data).find("a");
        imgLength = aLinks.length;
        aLinks.each(function(){
            href = $(this).attr("href");
            if(href.indexOf('.jpg') != -1) {
              var img = new Image();
              img.onload = function() {
                $(".feed").append("<div onclick=\"clickImage(this)\" id=\""+imgCounter+"\" class=\"img\" style=\"background:url('"+img.src+"');background-size:cover;\"></div>");
                imgCounter++;
              }
              item = {src: "http://need4bit.com"+href, w: 625, h: 417};
              items.push(item);
              img.src = "http://need4bit.com"+href;
            }
        });
      }
    });
}

function clickImage(selector) {
  console.log($(selector).attr('id'));
  openGallery($(selector).attr('id'));
}

function openGallery(x) {
  isGalleryUp = true;
  pswpElement = document.querySelectorAll('.pswp')[0];
  
  // define options (if needed)
  var options = {
      // history & focus options are disabled on CodePen        
      history: false,
      focus: false,

      showAnimationDuration: 0,
      hideAnimationDuration: 0
      
  };
  
  gallery = new PhotoSwipe( pswpElement, PhotoSwipeUI_Default, items);
  gallery.init();
  gallery.goTo(x);
}