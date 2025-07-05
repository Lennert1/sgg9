from django.urls import re_path

from api.WEBSOCKETS import consumers_basic, consumers_test

# the websocket urls.

websocket_urlpatterns = [
    re_path(r'ws/basic/$', consumers_basic.BasicWSServer.as_asgi()),
    re_path(r'ws/test/$', consumers_test.WSTest.as_asgi())
]
'''
(?P<XXXX>\w+)
corresponds to   self.scope["url_route"]["kwargs"]["XXXX"] in the connect method.
double check with django_part/django_part/asgi.py show_print=True
'''
