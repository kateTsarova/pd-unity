lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. "
TEXT_PLACE_HOLDER = "[]"


class Token:
    def __init__(self, key, parent_token, content_holder):
        self.key = key
        self.parent = parent_token
        self.children = []
        self.content_holder = content_holder

    def add_child(self, child):
        self.children.append(child)

    def show(self):
        print(self.key)
        for child in self.children:
            child.show()

    def render(self, mapping):
        content = ""
        for child in self.children:
            content += child.render(mapping)

        value = mapping[self.key]
        value = self.add_text(self.key, value)

        if len(self.children) != 0:
            value = value.replace(self.content_holder, content)

        return value

    def add_text(self, key, value):
        if key.find("btn") != -1:
            value = value.replace(TEXT_PLACE_HOLDER, lorem[:8])
        elif key.find("title") != -1:
            value = value.replace(TEXT_PLACE_HOLDER, lorem[:5])
        elif key.find("text") != -1:
            value = value.replace(TEXT_PLACE_HOLDER,
                                  lorem[:56])
        elif key.find("checkbox_active") != -1:
            value = value.replace(TEXT_PLACE_HOLDER,
                                  lorem[:8])
        elif key.find("checkbox_inactive") != -1:
            value = value.replace(TEXT_PLACE_HOLDER,
                                  lorem[:8])
        return value
