<!DOCTYPE html>
<html>

<head>
    <meta charset="utf-8" />
    <title>Вход</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.1/dist/css/bootstrap.min.css" rel="stylesheet"
        integrity="sha384-4bw+/aepP/YC94hEpVNVgiZdgIC5+VKNBQNGCHeKRQN+PtmoHDEXuppvnDJzQIu9" crossorigin="anonymous">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.1/dist/js/bootstrap.bundle.min.js"
        integrity="sha384-HwwvtgBNo3bZJJLYd8oVXjrBZt8cqVSpeBNS5n7C8IVInixGAoxmnlMuBnhbgrkm"
        crossorigin="anonymous"></script>
    <script type="text/javascript" src="../js/showHide.js"></script>
    <link rel="stylesheet" href="/styles/background.css" />
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <link rel="shortcut icon" href="/images/tractor.png">
</head>

<body>
    <div id="header_placer" class="header"></div>
    <section class="vh-100">
        <div class="container h-100">
            <div class="row d-flex justify-content-center align-items-center h-100">
                <div class="col-lg-12 col-xl-11">
                    <div class="card text-black" style="border-radius: 25px;">
                        <div class="card-body p-md-5">
                            <div class="row justify-content-center">
                                <div class="col-md-10 col-lg-6 col-xl-5 order-2 order-lg-1">
                                    <p class="text-center h1 fw-bold mb-5 mx-1 mx-md-4 mt-4">Вход</p>
                                    <form class="mx-1 mx-md-4" action="/login" method="post">
                                        <div class="d-flex flex-row align-items-center mb-4">
                                            <i class="fas fa-user fa-lg me-3 fa-fw"></i>
                                            <div class="form-outline flex-fill mb-0">
                                                <input type="text" id="login" name="login" class="form-control" />
                                                <label class="form-label" for="login">Логин</label>
                                                <span id="labelLogin" style="color:red"></span>
                                                {{if Model.ErrorFieldName == "login"}}
                                                <span style="color:red">{{Model.ErrorMsg}}</span>
                                                {{else}}
                                                {{endif}}
                                            </div>
                                        </div>
                                        <div class="d-flex flex-row align-items-center mb-4">
                                            <i class="fas fa-lock fa-lg me-3 fa-fw"></i>
                                            <div class="form-outline flex-fill mb-0">
                                                <input type="password" id="password" name="password"
                                                    class="form-control" />
                                                <label class="form-label" for="password">Пароль</label>
                                                <span id="labelPass" style="color:red"></span>
                                                {{if Model.ErrorFieldName == "password"}}
                                                <span style="color:red">{{Model.ErrorMsg}}</span>
                                                {{else}}
                                                {{endif}}
                                            </div>
                                        </div>
                                        <div class="form-check d-flex justify-content-center mb-5">
                                            <input class="form-check-input me-2" type="checkbox" id="remember_me"
                                                name="remember_me" />
                                            <label class="form-check-label" for="remember_me">
                                                Запомнить меня
                                            </label>
                                        </div>
                                        <div class="d-flex justify-content-center mx-4 mb-3 mb-lg-4">
                                            <button type="submit" class="btn btn-primary btn-lg">Войти</button>
                                        </div>
                                        <div class="d-flex justify-content-center mb-5">
                                            Нет аккаунта? <a href="/register"> Зарегистрироваться!</a>
                                        </div>
                                    </form>
                                </div>
                                <div class="col-md-10 col-lg-6 col-xl-7 d-flex align-items-center order-1 order-lg-2">
                                    <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-registration/draw1.webp"
                                        class="img-fluid" alt="Sample image">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <script type="text/javascript" src="/js/addHeader.js"></script>
    <script type="text/javascript" src="/js/validate.js"></script>
</body>

</html>