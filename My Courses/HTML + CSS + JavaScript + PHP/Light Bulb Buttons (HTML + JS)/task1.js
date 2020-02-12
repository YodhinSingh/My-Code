/*  this is called when a button is pressed to change the image to match that button */


window.onload = function() {
  var lightvaluelocation = document.getElementById('buttons');
  var lightvalue = lightvaluelocation.children;
  for (i = 0; i < lightvalue.length; i++) {

    lightvalue[0].onclick = function() {lights('1')};
    lightvalue[1].onclick = function() {lights('2')};
    lightvalue[2].onclick = function() {lights('3')};
    lightvalue[3].onclick = function() {lights('4')};
    lightvalue[4].onclick = function() {lights('5')};
  }

}

function lights(index){
  var changetext = document.getElementById('light');
  var imageNum = "light_";
  var indexNum = index.toString();
  var file = ".jpg";
  imageNum = imageNum.concat(indexNum, file);
  changetext.src = imageNum;
}

function light1() {
  var changetext = document.getElementById('light');
  changetext.src = "light_1.jpg";
}
function light2() {
  var changetext = document.getElementById('light');
  changetext.src = "light_2.jpg";
}
function light3() {
  var changetext = document.getElementById('light');
  changetext.src = "light_3.jpg";
}
function light4() {
  var changetext = document.getElementById('light');
  changetext.src = "light_4.jpg";
}
function light5() {
  var changetext = document.getElementById('light');
  changetext.src = "light_5.jpg";
}
