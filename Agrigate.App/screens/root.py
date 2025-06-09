from PySide6.QtWidgets import QTabWidget

from screens.production import Production
from screens.settings import Settings


class Root(QTabWidget):
    def __init__(self, parent=None):
        super().__init__(parent)

        self._production_tab = Production(self)
        self._settings_tab = Settings(self)

        self.addTab(self._production_tab, "Production")
        self.addTab(self._settings_tab, "Settings")