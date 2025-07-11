import utilities
from api.WEBSOCKETS.consumers_basic import BasicWSServer


class WSDungeon(BasicWSServer):
    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        print("init")
        self.commands = {

        }