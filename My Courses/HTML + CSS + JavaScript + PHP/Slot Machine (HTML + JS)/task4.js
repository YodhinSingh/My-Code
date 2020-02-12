/* two images on page. When the user hovers the mouse on the spin button,
the 2 images will cycle from a list of iamges. When the user removes their
mouse from the spin button, the images will stop. If the images are the same,
then the text changes to you win, else it says you lose*/

var images=["01.png", "02.png", "03.png", "04.png", "05.png", "06.png", "07.png", "08.png"];
var i =0;
var j = 1;

window.onload= function(){
  $('bar').observe("mouseover", spinner);
  $('bar').observe("mouseout", nospin);

}
function nospin(){
  this.innerHTML = "Move on to spin";
  this.style.backgroundColor = "white";
  if (i == j){
    $("result").innerHTML = "You Win!";
  }
  else{
    $("result").innerHTML = "You Lose!";
  }
  clearTimeout(timerID);
  clearTimeout(timerID2);
}

function spinner(){
  $("result").innerHTML = "Spin";
  this.innerHTML = "Move off to stop";
  this.style.backgroundColor = "grey";
  timerID = setInterval(pic1,50);
  timerID2 = setInterval(pic2,75);
}

function pic1(){
  $("img1").src = images[i];
  i++;
  if (i > 7) {
    i = 0;
  }
}
function pic2(){
  $("img2").src = images[j];
  j++;
  if (j > 7) {
    j = 0;
  }
}
