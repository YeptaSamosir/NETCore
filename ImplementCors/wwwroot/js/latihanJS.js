$(document).ready(function () {
    var t = $('#dataTable').DataTable({
        "filter": true,
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5]
                },
                text: '<span class="glyphicon glyphicon-pencil">Export Excel</span>',
                className: 'btn btn-default btn-xs',
                bom: true
            },
            {
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5]
                },
                className: 'btn btn-sm btn-outline-secondary',
                bom: true
            },
        ],
        "columnDefs": [{
            "searchable": false,
            "orderable": false,
            "targets": 0,
        }],

        "order": [[1, 'asc']],
        "ajax": {
            "url": '/person/getallpersons',
            "datatype": "json",
            "dataSrc": ""
        },
        "columns": [
            {
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                "data": "nik"
            },
            {
                "data": "namaDepan"
            },
            {
                "data": "telp",
                render: function (data) {
                    data = data.replace('0', '+62');
                    return data;
                }
            },
            {
                "data": "email"
            },
            {
                "data": "degree"
            },
            {
                mRender: function (data, type, full) {
                    return `<button class="btn btn-primary" title="Edit" data-toggle="modal" onClick="personDetail('${full.nik}')" data-target="#personDetailModal">
                                           Detail
                                        </button>
                            <button class="btn btn-danger" title="Delete" onClick="personDelete('${full.nik}')" data-target="#personDetailModal">
                                           Delete
                                        </button>`;
                }
            }
        ],
        'columnDefs': [{
            'targets': [0, 6],
            'orderable': false,
        }]

    });
    t.on('order.dt search.dt', function () {
        t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
});

function Register() {
    var data = {};
    data.nik = $("#nik").val();
    data.namaDepan = $("#namaDepan").val();
    data.namaBelakang = $("#namaBelakang").val();
    data.telp = $("#telp").val();
    data.tglLahir = $("#tglLahir").val();
    data.email = $("#email").val();
    data.password = $("#password").val();
    data.unversity_Id = $("#univ").val();
    data.degree = $("#degree").val();
    data.gpa = $("#gpa").val();

    $.ajax({
        url: 'https://localhost:44337/api/person/register',
        method: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(data),
        success: function (data) {
            location.reload();
            Swal.fire(
                'Good job!',
                'You clicked the button!',
                'success'
            )
            console.log("success")
            $('#dataTable').DataTable().ajax.reload(20000)
        },
        error: function () {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Data Yang anda Masukkan sudah Terdaftar',
            })
        }
    });
}


(function () {
    'use strict';
    window.addEventListener('load', function () {
        var forms = document.getElementsByClassName('needs-validation');
        var validation = Array.prototype.filter.call(forms, function (form) {
            form.addEventListener('submit', function (event) {
                if (form.checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                }
                form.classList.add('was-validated');
            }, false);
            form.addEventListener('submit', function (event) {
                if (form.checkValidity() === true) {
                    event.preventDefault();
                    Register();
                }
                form.classList.add('was-validated');
            }, false);
        });
    }, false);
})();



const personDetail = (url) => {
    let nik = url
    $.ajax({
        url: 'https://localhost:44337/api/person/getperson/' + nik
    }).done(res => {
        console.log(res.data[0])
        let personDetailContent = `
                                <ul>
                                    <li>NIK: ${res.data[0].nik} </li>
                                    <li>Nomor Telepon: ${res.data[0].telp} </li>
                                    <li>Email: ${res.data[0].email} </li>
                                    <li>Degree: ${res.data[0].degree} </li>
                                </ul>`;

        $('#personDetailModal .modal-body').html(personDetailContent);
        $('h5.modal-title').html(`${res.data[0].namaDepan}`);
    });
}

const personDelete = (nik) => {
    Swal.fire({
        title: 'Apakah anda yakin ingin menghapus?',
        showDenyButton: true,
        showCancelButton: true,
        confirmButtonText: 'Delete'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: 'https://localhost:44337/api/person/' + nik,
                method: 'DELETE',
                success: function (data) {
                    Swal.fire("Berhasil menghapus data", '', 'success')
                    $('#dataTable').DataTable().ajax.reload()
                }
            });
        } else if (result.isDenied) {
            Swal.fire('Data tidak jadi dihapus','','info')
        }
    })
}

$(document).ready(function () {
    $.ajax({
        url: '/person/getallpersons',
        type: "GET"
    }).done((result) => {
        console.log(result);
        var university_itd = result.filter(data => data.unversity_Id === "ITDEL").length;
        var university_itb = result.filter(data => data.unversity_Id === "ITB").length;
        var university_usu = result.filter(data => data.unversity_Id === "USU").length;
        console.log(university_itb);
        var options = {
            series: [university_itd, university_itb, university_usu],
            colors: ['#00C9A7', '#B39CD0', '#4D8076'],
            chart: {
                type: 'donut',
            },
            labels: ['ITD', 'ITB', 'USU'],
            responsive: [{
                breakpoint: 480,
                options: {
                    chart: {
                        width: 200
                    },
                    legend: {
                        position: 'bottom'
                    }
                }
            }]
        };

        var chart = new ApexCharts(document.querySelector("#chartuniv"), options);
        chart.render();

    }).fail((error) => {
        Swal.fire({
            title: 'Error!',
            text: 'Data Cannot Deleted',
            icon: 'Error',
            confirmButtonText: 'Next'
        })
    });
});

$(document).ready(function () {
    $.ajax({
        url: "/person/getallpersons",
        type: "GET"
    }).done((result) => {
        console.log(result);
        var d4 = result.filter(data => data.degree === "D4").length;
        var d3 = result.filter(data => data.degree=== "d3").length;
        var s2 = result.filter(data => data.degree === "S2").length;
        var s1 = result.filter(data => data.degree === "s1").length;
        //console.log(university_itd)
        var options = {
            series: [{
                data: [d3, d4, s1, s2]
            }],
            labels: ['D3', 'D4', 'S1', 'S2'],
            chart: {
                height: 350,
                type: 'bar',
            },
            plotOptions: {
                bar: {
                    borderRadius: 10,
                    dataLabels: {
                        position: 'top', // top, center, bottom
                    }
                }
            },
            xaxis: {
                categories: ["D3", "D4", "S1", "S2"],
                position: 'bottom',
                labels: {
                    style: {
                        fontSize: '15px'
                    }
                },
                axisBorder: {
                    show: true
                },
                axisTicks: {
                    show: true
                },
                crosshairs: {
                    fill: {
                        type: 'gradient',
                        gradient: {
                            colorFrom: ' #845EC2',
                            colorTo: '#4E8397',
                            stops: [0, 100],
                            opacityFrom: 0.4,
                            opacityTo: 0.5,
                        }
                    }
                },
                tooltip: {
                    enabled: false,
                }
            },
            yaxis: {
                axisBorder: {
                    show: true
                },
                axisTicks: {
                    show: true,
                },
                labels: {
                    show: true,
                    style: {
                        fontSize: '15px'
                    },
                    formatter: function (val) {
                        return val;
                    }
                }
            }
        };
        var chart = new ApexCharts(document.querySelector("#chartdegree"), options);
        chart.render();
    }).fail((error) => {
        Swal.fire({
            title: 'Error!',
            text: 'Data Cannot Deleted',
            icon: 'Error',
            confirmButtonText: 'Next'
        })
    });
});
//$.ajax({
//    url: 'https://localhost:44337/api/person/getperson/',
//    method : 'GET'
//}).done(res => {
//    let data
//    console.log(res)
//    $.each(res.data, (key, val) => {
//        data += `<tr>
//                            <td>${val.nik}</td>
//                            <td>${val.namaDepan}</td>
//                            <td>${val.telp}</td>
//                            <td>${val.email}</td>
//                            <td>${val.degree}</td>
//                            <td>
//                                <button
//                                    type="button"
//                                    class="item-detail btn btn-primary"
//                                    data-toggle="modal"
//                                    data-target="#personDetailModal"
//                                    onClick="personDetail('${val.nik}')">Detail</button>
//                                <button class="btn btn-danger">Hapus</button>
//                            </td>
//                        </tr>`
//    });
//    $('.data-person').html(data)
//}).fail(data =>console.log(data))

//const person = (nik) => {
//   $.ajax({
//        url: 'https://localhost:44337/api/person/getperson/${nik}',
//        method: 'GET'
//    }).done(res => {
//        console.log(res)
//        let data = `
//            <ul style= "list-style:none">
//                <li><b>NIK : </b> ${res.data.nik}</li>
//                <li><b>Firstname : </b> ${res.data.namaDepan}</li>
//                <li><b>Phone : </b> ${res.data.telp}</li>
//                <li><b>Email : </b> ${res.data.email}</li>
//                <li><b>Email : </b> ${res.data.degree}</li>
//            <ul>`
            
//        $('#personDetailModal .modal-body').html(data);
//        $('h5.modal-title').html(`${res.namaDepan}`);
//    });
//}