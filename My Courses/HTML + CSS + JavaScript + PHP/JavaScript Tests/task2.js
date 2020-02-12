/* this is called to highlight/unhighlight body text */

window.onload = function()
{
  var button = document.getElementById('button');
  button.onclick = test;
}

function test() {
  if (document.getElementById('button').value == "off"){
    var poem = document.getElementById('poem');
    var paras = poem.getElementsByTagName('p');
    for (var i = 0; i < paras.length; i++){
      paras[i].style.backgroundColor = "yellow";
    }
    button.innerHTML = "Click to unhighlight";
    document.getElementById('button').value = "on";
  }
  else{
    var poem = document.getElementById('poem');
    var paras = poem.getElementsByTagName('p');
    for (var i = 0; i < paras.length; i++){
      paras[i].style.backgroundColor = "silver";
    }
    button.innerHTML = "Click to highlight";
    document.getElementById('button').value = "off";
  }

}
