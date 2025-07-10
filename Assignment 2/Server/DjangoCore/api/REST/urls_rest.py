from django.urls import path, include
from api.REST import views

urlpatterns = [
    path("change_color/", views.change_color, name="change_color"),
    path("login/", views.login, name="login"),
    path("update_color/", views.update_color, name="update_color"),
    path("register/", views.register, name="register"),
]
