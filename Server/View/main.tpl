<!DOCTYPE html>
<html>

<head>
    <meta charset="utf-8">
    <title>Фермерские тракторы</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <link rel="stylesheet" href="/styles/username.css" />
    <link rel="stylesheet" href="/styles/background.css" />
    <link rel="shortcut icon" href="/images/tractor.png">
</head>

<body>
    <div id="header_placer" class="header"></div>
    <div class="container">
        <div class="row">
            <div class="col-md-12 text-center">
                <br>
                <h5>Ежедневная шутка для трактористов за триста: {{Model.Joke.TextJoke}} </h5>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 text-center">
                <br>
                <h4>Количество пользователей: <b>{{Model.CountUsers}}</b> Количество постов: <b>{{Model.CountPosts}}</b>
                </h4>
            </div>
        </div>
        <a href="/addtopic">
            <button class="btn btn-primary has-icon btn-block" type="submit" data-toggle="modal"
                data-target="#threadModal">
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none"
                    stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"
                    class="feather feather-plus mr-2">
                    <line x1="12" y1="5" x2="12" y2="19"></line>
                    <line x1="5" y1="12" x2="19" y2="12"></line>
                </svg>
                Опубликовать новую тему
            </button>
        </a>
        <div class="content_placer" id="content_placer"></div>
    </div>
</body>
<script type="text/javascript" src="/js/addHeader.js"></script>
<script type="text/javascript" src="/js/addContentMain.js"></script>

</html>