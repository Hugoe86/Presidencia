var timeout    = 500;
var closetimer = 0;
var ddmenuitem = 0;

function dropdown_open()
{  dropdown_canceltimer();
   dropdown_close();
   ddmenuitem = $(this).find('ul').css('visibility', 'visible');}

function dropdown_close()
{  if(ddmenuitem) ddmenuitem.css('visibility', 'hidden');}

function dropdown_timer()
{  closetimer = window.setTimeout(dropdown_close, timeout);}

function dropdown_canceltimer()
{  if(closetimer)
   {  window.clearTimeout(closetimer);
      closetimer = null;}}

$(document).ready(function()
{  $('#dropdown > li').bind('mouseover', dropdown_open)
   $('#dropdown > li').bind('mouseout',  dropdown_timer)});

document.onclick = dropdown_close;
