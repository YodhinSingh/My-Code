/* This code is called from the buttons in the task 3 html, and adds/removes items from the tree*/

window.onload = function()
{
  var add = document.getElementById('add');
  var del = document.getElementById('delete');
  add.onclick = addP;
  del.onclick = delP;
}

function addP() {
  var toadd = document.getElementById('output');
  var text = document.getElementById('input');
  if (text.value != "") {
    var newadd = document.createElement("p");
    newadd.innerHTML = text.value;
    toadd.appendChild(newadd);
  }
}

function delP() {
  var toadd = document.getElementById('output');
  var list = toadd.getElementsByTagName('p');
  if (list.length > 0) {
    toadd.removeChild(list[list.length-1])
  }
  else{
    alert("There is nothing to delete");
  }
}
