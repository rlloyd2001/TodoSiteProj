(function () {
  $.ajax({
      url : "http://localhost:60554/todosite/userlogin/showusers", 
      method: 'GET',
      success: function (data) {
        $("#content").html(data);
      },
      error: function(e, data) {
        console.log('error');
        console.log(arguments);
      }
  });
})()

