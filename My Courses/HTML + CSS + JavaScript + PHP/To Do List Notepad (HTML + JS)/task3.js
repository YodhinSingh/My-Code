/* This is called when user presses the add/ delete buttons. Using lists
it will add the input in the bottom. Remove will follow suite, unless the
user gives a specific number. Since its a list no need to worry about
a middle item being empty like an array.*/


var check = "";
window.onload= function(){
  var add = document.getElementById('add');
  var del = document.getElementById('del');
  var delnum = document.getElementById('delnum');
  var delchoice = document.getElementById('delchoice');
  var input = document.getElementById('input');
  add.onclick = toadd;
  del.onclick = todel;
  delnum.onclick = delsec;
  input.onclick = remove;
}

function remove(){
  this.value = "";
}

function toadd() {
  var list = document.getElementById('list');
  var input = document.getElementById('input');
  var addlist = document.createElement("li");
  addlist.innerHTML = input.value;
  list.appendChild(addlist);
}

function todel() {
  var list = document.getElementById('list');
  var lis = list.getElementsByTagName('li');
  list.removeChild(lis[lis.length-1]);
}
function delsec(){
  var delchoice = document.getElementById('delchoice');
  var numinput = parseInt(delchoice.value);
  var list = document.getElementById('list');
  var lis = list.getElementsByTagName('li');
  list.removeChild(lis[numinput-1]);
}
