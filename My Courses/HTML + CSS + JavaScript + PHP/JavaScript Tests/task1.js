/* called on by html task1 file to modify text based on button input*/

 window.onload = function() {
   var button1 = document.getElementById('button1');
   var button2 = document.getElementById('button2');
   var button3 = document.getElementById('button3');
   var button4 = document.getElementById('button4');
   var button5 = document.getElementById('button5');
   button1.onclick = makeGreen;
   button2.onclick = makeBlue;
   button3.onclick = makeMono;
   button4.onclick = makeSanSerif;
   button5.onclick = makeSerif;
 }

function makeGreen() {
  var paragraph = document.getElementById("paragraph");
  paragraph.style.color = "green";
}

function makeBlue() {
  var paragraph = document.getElementById("paragraph");
  paragraph.style.color = "blue";
}

function makeMono() {
  var paragraph = document.getElementById("paragraph");
  paragraph.style.fontFamily = "monospace";
}

function makeSanSerif() {
  var paragraph = document.getElementById("paragraph");
  paragraph.style.fontFamily = "sans-serif";
}

function makeSerif() {
  var paragraph = document.getElementById("paragraph");
  paragraph.style.fontFamily = "serif";
}
