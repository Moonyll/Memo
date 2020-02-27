$(document).ready(function () {

    // Get all picture details :
    $(".details").click(function () {

        // Get index of selected picture :
        var indexItem = $(".details").index(this);

        // Get selected picture elements :
        var mainPictureElements = getPictureElements($(this));

        // Catch the picture src :
        var selectedPictureFile = mainPictureElements[0];

        // Catch the picture id :
        var selectedPictureId = mainPictureElements[1];

        // Display selected picture elements :
        displayPictureElements(mainPictureElements);

        // Display button :
        displayButton(indexItem);

        // Manage the previous button :
        $(".prev").click(function () {

            // Previous index :
            var prevIndex = ((indexItem - 1) <= 0) ? 0 : (indexItem - 1);

            // Display button :
            displayButton(prevIndex);

            // Previous button :
            var prevElementButton = $(".details").eq(prevIndex);

            // Previous picture elements :
            var prevPictureElements = getPictureElements(prevElementButton);

            // Previous src :
            selectedPictureFile = prevPictureElements[0];

            // Previous id :
            selectedPictureId = prevPictureElements[1];

            // Display previous picture elements :
            displayPictureElements(prevPictureElements);

            // Decrement index :
            indexItem--;
        });

        // Manage the next button :
        $(".next").click(function () {

            // Next index :
            var nextIndex = ((indexItem + 1) >= 7) ? 7 : (indexItem + 1);

            // Display button :
            displayButton(nextIndex);

            // Next button :
            var nextElementButton = $(".details").eq(nextIndex);

            // Next picture elements :
            var nextPictureElements = getPictureElements(nextElementButton);

            // Next src :
            selectedPictureFile = nextPictureElements[0];

            // Next id :
            selectedPictureId = nextPictureElements[1];

            // Display next picture elements :
            displayPictureElements(nextPictureElements);

            // Increment index :
            indexItem++;
        });

        // Get picture selected elements :
        function getPictureElements(button) {

            // Get picture source to display in the modal :
            var source = button.parent().prev().attr("src");

            // Get the selected id in the modal :
            var id = parseInt(button.prev().prev().children(".pictureId").text());

            // Get the selected title image in the modal :
            var title = button.prev().prev().children(".pictureTitle").text();

            // Get the selected alt. title image in the modal :
            var alternate = button.parent().prev().attr("title");

            // Get the selected description image in the modal :
            var description = button.prev().prev().children(".pictureDescription").text();

            // Get the selected category image in the modal :
            var category = button.prev().prev().children(".pictureCategory").text();

            return pictureElements = new Array(source, id, title, alternate, description, category);
        }

        // Display selected elements :
        function displayPictureElements(pictureElements) {

            // Display the selected image in the modal :
            $(".displayPicture").attr("src", pictureElements[0]);

            // Display the selected id in the modal :
            $("#pictureId").text(pictureElements[1]);

            // Display the selected image title in the modal :
            $("#pictureTitle").text(pictureElements[2]);

            // Display the alt. title in the modal :
            $(".displayPicture").attr("title", pictureElements[3]);

            // Display the selected image description in the modal :
            $("#pictureDescription").text(pictureElements[4]);

            // Display the selected image category in the modal :
            $("#pictureCategory").text(pictureElements[5]);
        }

        // Display prev / next buttons :
        function displayButton(buttonIndex) {

            // Hide prev button on first picture :
            (buttonIndex == 0) ? $(".prev").hide() : $(".prev").show();

            // Hide next button on last picture :
            (buttonIndex == 7) ? $(".next").hide() : $(".next").show();
        }

        // Ajax call to edit the picture :
        $(".editPicture").click(function () {
            $.ajax({
                url: '@Url.Action("pictureEdit", "Pictures")',
                type: 'GET',
                dataType: 'html',
                contentType: 'application/json; charset=utf-8',
                data: { id: selectedPictureId },

                // Id is found :
                success: function () {

                    // Redirect to edition view page to update picture :
                    var urlEdit = '@Url.Action("pictureEdit", "Pictures")' + '/' + selectedPictureId;
                    console.log(urlEdit);
                    window.location.href = urlEdit;
                },

                // Id is not found :
                error: function () {

                    // Redirect to error view page :
                    window.location.href = '@Url.Action("pictureError", "Pictures")';
                }
            });
        });

        // Ajax call to display exifs picture data :
        $(".getExifs").click(function () {
            $.ajax({
                url: '@Url.Action("GetExifs", "Pictures")',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: { pictureFile: selectedPictureFile },

                // src is found :
                success: function (result) {

                    // Display exifs picture data :

                    $("#pictureCameraMake").text("\u2003" + result.pictureCameraMake);

                    $("#pictureCameraModel").text("\u2003" + result.pictureCameraModel);

                    $("#pictureOriginalDateTime").text("\u2003" + result.pictureOriginalDateTime);

                    $("#pictureApertureValue").text("\u2003" + result.pictureApertureValue);

                    $("#pictureExposureTime").text("\u2003" + result.pictureExposureTime);

                    $("#pictureIsoSpeedRatings").text("\u2003" + "ISO " + result.pictureIsoSpeedRatings);

                    $("#pictureFocalLength").text("\u2003" + result.pictureFocalLength);

                    $("#pictureFlash").text("\u2003" + result.pictureFlash);

                    $("#pictureDimensions").text("\u2003" + result.pictureWidth + " X " + result.pictureHeight)
                        ;
                    $("#pictureFileSize").text("\u2003" + result.pictureFileSize);

                    console.log(result);
                },

                // src is not found :
                error: function () {

                    // Redirect to error view page :
                    window.location.href = '@Url.Action("pictureError", "Pictures")';
                }
            });
        });
    });
    //
        //If needed new code here.
    //
});