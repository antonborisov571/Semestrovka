<!DOCTYPE html>
<html>

<head>
    <meta charset="utf-8">
    <title>Новые посты</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.1/dist/css/bootstrap.min.css" rel="stylesheet"
        integrity="sha384-4bw+/aepP/YC94hEpVNVgiZdgIC5+VKNBQNGCHeKRQN+PtmoHDEXuppvnDJzQIu9" crossorigin="anonymous">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.1/dist/js/bootstrap.bundle.min.js"
        integrity="sha384-HwwvtgBNo3bZJJLYd8oVXjrBZt8cqVSpeBNS5n7C8IVInixGAoxmnlMuBnhbgrkm"
        crossorigin="anonymous"></script>
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <link rel="stylesheet" href="/styles/background.css" />
    <link rel="stylesheet" href="/styles/username.css" />
    <link rel="shortcut icon" href="/images/tractor.png">
</head>

<body>
    <div id="header_placer" class="header"></div>
    <div class="container">
        <div class="container mt-4">
            <div class="row">
                <div class="col-12">
                    <h2>Новые посты</h2>
                    {{for post in Model}}
                    <div class="card mb-4" id="post_{{post.PostInfo.Id}}">
                        <div class="card-body" id="post_body_{{post.PostInfo.Id}}">
                            <div class="d-flex w-100 justify-content-between">
                                <a href="/topic/{{post.PostInfo.TopicId}}/messages/{{post.PostInfo.Id}}">
                                    <h5 class="mb-1" id="post_name_{{post.PostInfo.Id}}">{{post.PostInfo.PostName}}</h5>
                                </a>
                            </div>
                            <p class="mb-1 w-100" id="post_desc_{{post.PostInfo.Id}}">{{post.PostInfo.Description}}</p>
                            <div class="d-flex justify-content-between align-items-center">
                                <div class="user-info">
                                    <img class="rounded" src="{{post.User.UrlImage}}" alt="Фото пользователя"
                                        style="height:48px;width:48px">
                                    <a class="user-name" href="/users/{{post.User.Id}}"><span class="user-name"
                                            id="post_user_{{post.PostInfo.Id}}"
                                            style="font-weight:bold">{{post.User.Login}}</span></a>
                                </div>
                                <div class="post-info">
                                    <p class="mb-0" id="post_time_{{post.PostInfo.Id}}">Дата публикации: {{
                                        post.PostInfo.DateDispatch }}</p>
                                </div>
                            </div>
                        </div>
                    </div>
                    {{endfor}}
                </div>
                <div class="col-md-4">
                </div>
            </div>
        </div>
</body>
<script type="text/javascript" src="/js/addHeader.js"></script>

</html>