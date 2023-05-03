var UploadGroupApp = angular.module("UploadGroupApp", ['ui.bootstrap']);

UploadGroupApp.controller("ApiUploadGroupController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;


    // Inital Grid view load 
    var compid = $('#txtcompid').val();
    $http.get('/api/ApiUploadGroup/GetGroupData/', {
        params: {
            Compid: compid,
        }
    }).success(function (data) {
        if (data[0].count == 1) {
            $scope.UploadGroupData = null;
        } else {
            $scope.UploadGroupData = data;
        }
        $scope.loading = false;
    });




    $scope.toggleEdit = function () {
        this.testitem.editMode = !this.testitem.editMode;
    };



    $scope.toggleEdit_Cancel = function () {
        this.testitem.editMode = !this.testitem.editMode;
        //Grid view load 
        var compid1 = $('#txtcompid').val();
        $http.get('/api/ApiUploadGroup/GetGroupData/', {
            params: {
                Compid: compid1,
            }
        }).success(function (data) {
            $scope.UploadGroupData = data;
            $scope.loading = false;
        });
    };





    //Add grid level data
    $scope.addrow = function (event) {
        $scope.loading = false;
        event.preventDefault();

        this.newChild.COMPID = $('#txtcompid').val();
        this.newChild.INSUSERID = $('#txtInsertUserid').val();
        this.newChild.INSLTUDE = $('#latlon').val();
        this.newChild.GROUPNM = $('#GroupNameID').val();
        if (this.newChild.GROUPNM != "") {
            $http.post('/api/ApiUploadGroup/Add', this.newChild).success(function (data, status, headers, config) {

                //Grid view load 
                var compid2 = $('#txtcompid').val();
                $http.get('/api/ApiUploadGroup/GetGroupData/', {
                    params: {
                        Compid: compid2,
                    }
                }).success(function (data) {
                    $scope.UploadGroupData = data;
                    $scope.loading = false;
                });


                if (data.GROUPID != 0) {
                    $('#GroupNameID').val("");
                    alert("Create Successfully !!");
                    //$scope.UploadGroupData.push({ ID: data.ID, GROUPID: data.GROUPID, GROUPNM: data.GROUPNM });
                } else {
                    $('#GroupNameID').val("");
                    alert("Duplicate name will not create!");
                }

            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
                $scope.loading = false;
            });

        }
        else {
            $('#GroupNameID').val("");
            alert("Please input pharma name field !!");
        }
};







//Update to grid level data (save a record after edit)
$scope.update = function () {
    $scope.loading = true;
    var frien = this.testitem;

    this.testitem.COMPID = $('#txtcompid').val();
    this.testitem.INSUSERID = $('#txtInsertUserid').val();
    this.testitem.INSLTUDE = $('#latlon').val();
    var groupName = this.testitem.GROUPNM;

    if (groupName != "") {
        $http.post('/api/ApiUploadGroup/Update', this.testitem).success(function (data) {
            if (data.GROUPID != 0) {
                alert("Saved Successfully!!");
            } else {
                alert("Duplicate data entered will not create!");
            }

            frien.editMode = false;

            //Grid view load 
            var compid4 = $('#txtcompid').val();
            $http.get('/api/ApiUploadGroup/GetGroupData/', {
                params: {
                    Compid: compid4,
                }
            }).success(function (data) {
                $scope.UploadGroupData = data;
            });

            $scope.loading = false;

        }).error(function (data) {
            $scope.error = "An Error has occured while Saving Friend! " + data;
            $scope.loading = false;

        });
    }
};







//Delete grid level data.
$scope.deleteItem = function () {
    $scope.loading = true;
    var id = this.testitem.ID;

    if (id != "") {
        this.testitem.COMPID = $('#txtcompid').val();
        this.testitem.INSUSERID = $('#txtInsertUserid').val();
        this.testitem.INSLTUDE = $('#latlon').val();

        $http.post('/api/ApiUploadGroup/Delete', this.testitem).success(function (data) {

            var getChildDataForDeleteMasterCategory = data.GetChildDataForDeleteMasterCategory;
            if (getChildDataForDeleteMasterCategory == 1) {
                $scope.loading = false;
                alert("Please firstly delete Group wise Contact data first!!");
            }
            else {
                $.each($scope.UploadGroupData, function (i) {
                    if ($scope.UploadGroupData[i].ID === id) {
                        $scope.UploadGroupData.splice(i, 1);
                        return false;
                    }
                });
                $scope.loading = false;
                alert("Delete Successfully!!");
            }
        }).error(function (data) {
            $scope.error = "An Error has occured while delete posts! " + data;
            $scope.loading = false;
        });
    }
};


});