<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script>
    $(document).ready(function() {
        $('#cv-upload').on('change', function() {
            var fileName = $(this).val().split('\\').pop(); // Get the file name from the path
            if (fileName) {
                $(this).next('.custom-file-upload').find('.file-name').text(fileName); // Update the text inside the box
            } else {
                $(this).next('.custom-file-upload').find('.file-name').text('ფაილი არ არის'); // Reset text if no file chosen
            }
        });
    });
</script>
