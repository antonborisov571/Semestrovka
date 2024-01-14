<div class="container mt-4">
    <div class="row">
        <div class="col-12">
            <h4>Главная > Форум</h4>
            <h2>Общие</h2>
            {{for topic in Model.Topics}}
            <div class="card mb-4" id="topic_{{topic.Id}}">
                <div class="card-body" id="topic_body_{{topic.Id}}">
                    <div class="d-flex w-100 justify-content-between">
                        <a href="/topic/{{topic.Id}}">
                            <h5 class="mb-1" id="topic_name_{{topic.Id}}">{{topic.TopicName}}</h5>
                        </a>
                    </div>
                    <p class="mb-1 w-100" id="topic_desc_{{topic.Id}}">{{topic.Description}}</p>
                    <div class="d-flex justify-content-between align-items-center">
                        <div class="user-info">
                            <img class="rounded" src="{{topic.User.UrlImage}}" alt="Фото пользователя"
                                style="height:48px;width:48px">
                            <a class="user-name" href="/users/{{topic.User.Id}}">
                                <span class="user-name" id="topic_user_{{topic.Id}}"
                                    style="font-weight:bold">{{topic.User.Login}}</span>
                            </a>
                        </div>
                        <div class="d-flex flex-column col-6 col-sm-4">
                            <div class="post-info">
                                <p class="mb-0" id="topic_count_posts_{{topic.Id}}">Посты: {{topic.CountPosts}}</p>
                            </div>
                            <div class="post-info">
                                <p class="mb-0" id="topic_datetime_{{topic.Id}}">Дата публикации: {{ topic.Time }}</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            {{endfor}}
        </div>
    </div>
</div>