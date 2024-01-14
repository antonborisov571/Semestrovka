<!DOCTYPE html>
<html>

<head>
    <meta charset="utf-8">
    <title>Поиск: {{Model.TextFind}}</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.1/dist/css/bootstrap.min.css" rel="stylesheet"
        integrity="sha384-4bw+/aepP/YC94hEpVNVgiZdgIC5+VKNBQNGCHeKRQN+PtmoHDEXuppvnDJzQIu9" crossorigin="anonymous">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.1/dist/js/bootstrap.bundle.min.js"
        integrity="sha384-HwwvtgBNo3bZJJLYd8oVXjrBZt8cqVSpeBNS5n7C8IVInixGAoxmnlMuBnhbgrkm"
        crossorigin="anonymous"></script>
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <link rel="stylesheet" href="/styles/background.css" />
    <link rel="stylesheet" href="/styles/star.css" />
    <link rel="stylesheet" href="/styles/heart.css" />
    <link rel="stylesheet" href="/styles/username.css" />
    <link rel="shortcut icon" href="/images/tractor.png">
</head>

<body>
    <div id="header_placer" class="header"></div>
    <div class="container">
        <div class="container mt-4">
            <div class="row">
                <div class="col-12">
                    {{if Model.HasTopics}}
                    <h2>{{if Model.Topics.ToList().Count > 0 }} Темы {{else}} Тем нет {{endif}}</h2>
                    {{for topic in Model.Topics}}
                    <div class="card mb-4" id="topic_{{topic.TopicInfo.Id}}">
                        <div class="card-body" id="topic_body_{{topic.TopicInfo.Id}}">
                            <div class="d-flex w-100 justify-content-between">
                                <a href="/topic/{{topic.TopicInfo.Id}}">
                                    <h5 class="mb-1" id="topic_name_{{topic.TopicInfo.Id}}">
                                        {{topic.TopicInfo.TopicName}}</h5>
                                </a>
                            </div>
                            <p class="mb-1 w-100" id="topic_desc_{{topic.TopicInfo.Id}}">{{topic.TopicInfo.Description}}
                            </p>
                            <div class="d-flex justify-content-between align-items-center">
                                <div class="user-info">
                                    <img class="rounded" src="{{topic.User.UrlImage}}" alt="Фото пользователя"
                                        style="height:48px;width:48px">
                                    <a class="user-name" href="/users/{{topic.User.Id}}"><span class="user-name"
                                            id="topic_user_{{topic.TopicInfo.Id}}"
                                            style="font-weight:bold">{{topic.User.Login}}</span></a>
                                </div>
                                <div class="post-info">
                                    <p class="mb-0" id="topic_datetime_{{topic.TopicInfo.Id}}">Дата публикации: {{
                                        topic.TopicInfo.DateDispatch }}</p>
                                </div>
                            </div>
                        </div>
                    </div>
                    {{endfor}}
                    {{else}}{{endif}}
                    {{if Model.HasPosts}}
                    <h2>{{if Model.Posts.ToList().Count > 0 }} Посты {{else}} Постов нет {{endif}}</h2>
                    {{for post in Model.Posts}}
                    <div class="card mb-4" id="post_{{post.PostInfo.Id}}">
                        <div class="card-body" id="post_body_{{post.PostInfo.Id}}">
                            <div class="d-flex w-100 justify-content-between">
                                <a href="{{post.PostInfo.TopicId}}/messages/{{post.PostInfo.Id}}">
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
                    {{else}}{{endif}}
                    {{if Model.HasMessages}}
                    <h2>{{if Model.Messages.ToList().Count > 0 }} Сообщения {{else}} Сообщений нет {{endif}}</h2>
                    {{for message in Model.Messages}}
                    <div class="container mt-3">
                        <div class="row">
                            <div class="col-md-3">
                                <div class="card mt-4">
                                    <div class="card-body" id="message_userinfo_{{message.MessageInfo.Id}}">
                                        <img class="rounded" src="{{message.User.UrlImage}}" alt="Фото пользователя"
                                            style="height:120px;width:120px">
                                        <a class="user-name" href="/users/{{message.User.Id}}">
                                            <h5 class="card-title">{{message.User.Login}}</h5>
                                        </a>
                                        <p class="card-text">Трактор: {{message.User.TractorName}}</p>
                                        <p class="card-text">Репутация: {{message.User.Reputation}}</p>
                                        <p class="card-text">С нами с:
                                            {{message.User.DateRegistration.ToShortDateString()}}</p>
                                        <p class="card-text">Последняя активность:
                                            {{message.User.LastOnline.ToString("g")}}</p>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-9">
                                <div class="mt-4">
                                    <div class="card mb-2">
                                        <div class="card-body">
                                            <div class="d-flex justify-content-between">
                                                <div>
                                                    <small class="text-muted"
                                                        id="message_time_{{message.MessageInfo.Id}}">{{message.MessageInfo.DateDispatch}}</small>
                                                </div>
                                                <p class="mb-1 w-100" id="message_desc_{{message.MessageInfo.Id}}">
                                                    {{message.MessageInfo.TextMessage}}</p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            {{endfor}}
                        </div>
                    </div>
                </div>
                {{else}}{{endif}}
            </div>
        </div>
    </div>
</body>
<script type="text/javascript" src="/js/addHeader.js"></script>
<script type="text/javascript" src="/js/addPost.js"></script>

</html>