const template = document.createElement("template");
template.innerHTML = `
 <style>
  .user-card {
		font-family: 'Arial', sans-serif;
		background: #11121a;
		width: 500px;
		display: grid;
		grid-template-columns: 1fr 2fr;
		grid-gap: 10px;
		margin-bottom: 15px;
		border-bottom: #42434a 1px solid;
	}

	.user-card img {
		width: 100%;
	}

	.user-card button {
		cursor: pointer;
		background: #e6e6ef;
		color: #11121a;
		border: 0;
		border-radius: 5px;
		padding: 5px 10px;
	}
  </style>
  <div class="user-card">
    <img />
    <div>
      <h3></h3>
      <div class="info">
        <p><slot name="designer" /></p>
        <p><slot name="style" /></p>
        <p><slot name="description" /></p>
      </div>
      <button id="toggle-info">Verberg details</button>
    </div>
  </div>
`;
class UserCard extends HTMLElement {
  constructor() {
    super();
    this.showInfo = true;
    this.attachShadow({mode: "open"});
    this.shadowRoot.appendChild(template.content.cloneNode(true));
    this.shadowRoot.querySelector("h3").innerText = this.getAttribute("name");
    this.shadowRoot.querySelector("img").src = this.getAttribute("image");
  }

  connectedCallback() {
    this.shadowRoot.querySelector("#toggle-info").addEventListener("click", () => this.toggleInfo());
  }

  disconnectedCallback() {
    //this.shadowRoot.querySelector("#toggle-info").removeEventListener();
  }

  toggleInfo() {
    this.showInfo = ! this.showInfo;
    const info = this.shadowRoot.querySelector(".info");
    const toggleButton = this.shadowRoot.querySelector("#toggle-info");

    if(this.showInfo) {
      info.style.display = "block";
      toggleButton.innerText = "Verberg details";
    } else {
      info.style.display = "none";
      toggleButton.innerText = "Toon details";
    }
  }
}

window.customElements.define('user-card', UserCard);
