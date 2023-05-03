var UploadEditContactApp = angular.module("UploadEditContactApp", ['ui.bootstrap']);

UploadEditContactApp.controller("ApiUploadEditController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;


    $scope.search = function (event) {

        $scope.loading = true;
        event.preventDefault();

        var compid = $('#txtcompid').val();
        var changedDDGroupId = $('#ddlGroupnameID option:selected').val();

        $http.get('/api/ApiUploadEdit/GetContactData/', {
            params: {
                Compid: compid,
                Groupid: changedDDGroupId,
            }
        }).success(function (data) {
            if (data[0].count == 1) {
                $scope.ContactData = null;
            } else {
                $scope.ContactData = data;
            }
            $scope.loading = false;
        });

    };




    $scope.toggleEdit = function () {
        this.testitem.editMode = !this.testitem.editMode;
    };



    $scope.toggleEdit_Cancel = function () {
        this.testitem.editMode = !this.testitem.editMode;

        //Grid view load 
        var compid = $('#txtcompid').val();
        var changedDDGroupId = $('#ddlGroupnameID option:selected').val();

        $http.get('/api/ApiUploadEdit/GetContactData/', {
            params: {
                compId: compid,
                groupId: changedDDGroupId,
            }
        }).success(function (data) {
            $scope.ContactData = data;
            $scope.loading = false;
        });
    };




    ////Add grid level data
    //$scope.addrow = function (event) {
    //    $scope.loading = false;
    //    event.preventDefault();

    //    this.newChild.COMPID = $('#txtcompid').val();
    //    this.newChild.INSUSERID = $('#txtInsertUserid').val();
    //    this.newChild.INSLTUDE = $('#latlon').val();
    //    this.newChild.NAME = $('#NameID').val();
    //    this.newChild.EMAIL = $('#EmailID').val();
    //    this.newChild.MOBILENO = $('#MobileNoID').val();
    //    this.newChild.ADDRESS = $('#AddressID').val();
    //    this.newChild.GROUPID = $('#ddlGroupnameID option:selected').val();

    //    if ((this.newChild.GROUPID != "" || this.newChild.GROUPID != 0) && this.newChild.NAME != "" && this.newChild.EMAIL != "" && this.newChild.MOBILENO!="") {
    //        $http.post('/api/ApiUploadExchangeContactController/grid/addData', this.newChild).success(function (data, status, headers, config) {

    //            //Grid view load 
    //            var compid2 = $('#txtcompid').val();
    //            var changedDDGroupId2 = $('#ddlGroupnameID option:selected').val();

    //            $http.get('/api/ApiUploadExchangeContactController/GetContactData/', {
    //                params: {
    //                    compId: compid2,
    //                    groupId: changedDDGroupId2,
    //                }
    //            }).success(function (data) {
    //                $scope.ContactData = data;
    //                $scope.loading = false;
    //            });


    //            if (data.MEDIID != 0) {
    //                $('#NameID').val("");
    //                $('#EmailID').val("");
    //                $('#MobileNoID').val("");
    //                $('#AddressID').val("");
    //                alert("Create Successfully !!");
    //                //$scope.ContactData.push({ ID: data.ID, GROUPID: data.GROUPID, MEDIID: data.MEDIID, MEDINM: data.MEDINM, PHARMAID: data.PHARMAID, GHEADID: data.GHEADID });
    //            } else {
    //                $('#NameID').val("");
    //                $('#EmailID').val("");
    //                $('#MobileNoID').val("");
    //                $('#AddressID').val("");
    //                alert("Duplicate name will not create!");
    //            }

    //        }).error(function () {
    //            $scope.error = "An Error has occured while loading posts!";
    //            $scope.loading = false;
    //        });

    //    }
    //    else if (this.newChild.GROUPID == 0 || this.newChild.GROUPID == "") {
    //        $('#ddlGroupnameID').val("");
    //        $('#NameID').val("");
    //        $('#EmailID').val("");
    //        $('#MobileNoID').val("");
    //        $('#AddressID').val("");
    //        alert("Please select group name first !!");
    //    }
    //    else {
    //        alert("Please input grid level data !!");
    //    }
    //};








    //Update to grid level data (save a record after edit)
    $scope.update = function () {
        $scope.loading = true;
        var frien = this.testitem;

        this.testitem.COMPID = $('#txtcompid').val();
        this.testitem.INSUSERID = $('#txtInsertUserid').val();
        this.testitem.INSLTUDE = $('#latlon').val();
        this.testitem.From_GROUPID = $('#ddlGroupnameID option:selected').val();

        if (this.testitem.From_GROUPID != "") {
            $http.post('/api/ApiUploadEdit/Update', this.testitem).success(function (data) {
                if (data.CheckValidation == 1) {
                    alert("Please input valid email and mobile number!");
                }

                if (data.CheckPreviousData == 1) {
                    alert("Duplicate data entered will not create!");
                } else {
                    alert("Update Successfully!!");
                }

                frien.editMode = false;

                //Grid view load 
                var compid3 = $('#txtcompid').val();
                var changedDDGroupId3 = $('#ddlGroupnameID option:selected').val();

                $http.get('/api/ApiUploadEdit/GetContactData/', {
                    params: {
                        compId: compid3,
                        groupId: changedDDGroupId3,
                    }
                }).success(function (data) {
                    $scope.ContactData = data;
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

            $http.post('/api/ApiUploadEdit/Delete', this.testitem).success(function (data) {

                $.each($scope.ContactData, function (i) {
                    if ($scope.ContactData[i].ID === id) {
                        $scope.ContactData.splice(i, 1);
                        return false;
                    }
                });
                $scope.loading = false;
                alert("Delete Successfully!!");

            }).error(function (data) {
                $scope.error = "An Error has occured while delete posts! " + data;
                $scope.loading = false;
            });
        }
    };



});