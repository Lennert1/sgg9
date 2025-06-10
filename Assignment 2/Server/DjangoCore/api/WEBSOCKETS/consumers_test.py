import utilities
from api.WEBSOCKETS.consumers_basic import BasicWSServer


class WSTest(BasicWSServer):
    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        print("init")
        self.commands = {
            "TEST": self.handle_test,
        }

    async def handle_test(self, data):
        print(data)

        a = data.get("a", 0)
        b = data.get("b", 0)
        result = a * b
        await self.send_message_to_client(f"Ergebnis: {result}", identifier="multiply", extra_message="Server")
