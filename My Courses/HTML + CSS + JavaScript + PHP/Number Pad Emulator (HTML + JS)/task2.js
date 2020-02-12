/* this is called whenever a user presses a button and adds the corresponding
digit to the calculator/ removes it. */

/* write your code here */

window.onload= function(){
  var button0 = document.getElementById('0');
  var button1 = document.getElementById('1');
  var button2 = document.getElementById('2');
  var button3 = document.getElementById('3');
  var button4 = document.getElementById('4');
  var button5 = document.getElementById('5');
  var button6 = document.getElementById('6');
  var button7 = document.getElementById('7');
  var button8 = document.getElementById('8');
  var button9 = document.getElementById('9');
  var del = document.getElementById('delete');
  button0.onclick = add;
  button1.onclick = add;
  button2.onclick = add;
  button3.onclick = add;
  button4.onclick = add;
  button5.onclick = add;
  button6.onclick = add;
  button7.onclick = add;
  button8.onclick = add;
  button9.onclick = add;
  del.onclick = delnum;
}

function add(){
  var adding = document.getElementById('entry');
  adding.innerHTML = adding.innerHTML + this.innerHTML;
}

function delnum(){
  var adding = document.getElementById('entry').innerHTML;
  document.getElementById('entry').innerHTML = adding.slice(0,-1);
}
